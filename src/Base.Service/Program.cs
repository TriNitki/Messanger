using Base.Authentication.Integration;
using Base.Service.Options;
using NLog;
using NLog.Web;
using Packages.Application.Consul;
using System.Reflection;
using Base.Authentication.Core.Options;
using Base.Authentication.UseCases.Commands.Login;
using Base.DataAccess;
using Base.Permission.UseCases;
using Packages.Application.Data.DI;
using Base.Authentication.UseCases.Abstractions;
using Base.DataAccess.Repositories;
using Base.Permission.UseCases.Abstractions;
using Base.Service.Infrastructure;
using Base.Authorization;
using Base.Authorization.Permission;
using Base.Permission.Clients;
using Microsoft.AspNetCore.Authorization;

namespace Base.Service;

public class Program
{
    public static async Task Main(string[] args)
    {
        var logger = LogManager.Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        try
        {
            logger.Debug("init main");
            var builder = ConfigureApplicationBuilder(args);
            await RunApp(builder);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error occurred when starting the host");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static WebApplicationBuilder ConfigureApplicationBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        var services = builder.Services;
        var cfg = builder.Configuration;

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddConsulIntegration(cfg);
        services.AddHealthChecks();

        services.AddCors(opts =>
        {
            opts.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddJwtBearerAuthentication(
            cfg.GetSection(nameof(SecurityOptions))[nameof(SecurityOptions.SecretKey)]!);

        AddAuthorization(services, cfg);

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        services.AddSwagger(xmlFilePath);

        ConfigureDI(services, cfg);

        return builder;
    }

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAutoMapper(typeof(DbMappingProfile).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([
            typeof(LoginCommand).Assembly,
            typeof(CheckFeatureAccessQuery).Assembly
        ]));
        services.AddDataContext<DataBaseContext>(configuration);

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        services.Configure<List<string>>(configuration.GetRequiredSection("DefaultUserRoles"));
    }

    private static void AddAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        // Permission based authorization setup
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionHandler>();
        services.AddTransient<IFeatureAccessProvider, InternalFeatureAccessProvider>();

        // Authorized user setup
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddHttpContextAccessor();

        // JWT setup
        var securityOptionsSection = configuration.GetSection(nameof(SecurityOptions));
        services.Configure<SecurityOptions>(x =>
        {
            x.SecretKey = securityOptionsSection[nameof(SecurityOptions.SecretKey)] 
                          ?? throw new ArgumentNullException(null, "Secret key is not set");

            x.AccessTokenLifetimeInMinutes
                = securityOptionsSection
                    .GetValue(nameof(SecurityOptions.AccessTokenLifetimeInMinutes), 15);

            x.RefreshTokenLifetimeInMinutes
                = securityOptionsSection
                    .GetValue(nameof(SecurityOptions.RefreshTokenLifetimeInMinutes), 180);
        });

        services.Configure<PasswordOptions>(x =>
        {
            x.Salt = securityOptionsSection.GetSection(nameof(PasswordOptions))[nameof(PasswordOptions.Salt)]
                     ?? throw new ArgumentNullException(null, "Password salt is not set");
        });
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
                      ?? throw new ArgumentNullException(null, "Service name is not set");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DataBaseContext>();
            await context.Database.EnsureCreatedAsync();
        }

        app.UseRouting();
        app.UseCors();

        app.MapHealthChecks("/health").AllowAnonymous();
        app.MapGet(string.Empty, async ctx => await ctx.Response.WriteAsync(appName)).AllowAnonymous();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}