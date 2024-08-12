using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using MSG.Security.Authentication.Core.Options;
using MSG.Security.Authentication.Integration;
using MSG.Security.Authentication.UseCases.Abstractions;
using MSG.Security.Authentication.UseCases.Commands.Login;
using MSG.Security.Authorization;
using MSG.Security.Authorization.Client;
using MSG.Security.Authorization.Permission;
using MSG.Security.DataAccess;
using MSG.Security.DataAccess.Repositories;
using MSG.Security.Permission.Clients;
using MSG.Security.Permission.UseCases;
using MSG.Security.Permission.UseCases.Abstractions;
using MSG.Security.Service.Infrastructure;
using MSG.Security.Service.Options;
using NLog;
using NLog.Web;
using Packages.Application.Consul;
using Packages.Application.Data.DI;

namespace MSG.Security.Service;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var logger = LogManager.Setup()
            .LoadConfigurationFromAppSettings()
            .GetCurrentClassLogger();

        try
        {
            logger.Debug("init main");
            var builder = ConfigureApp(args);
            logger.Debug("Application configured");
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

    private static WebApplicationBuilder ConfigureApp(string[] args)
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
        services.AddScoped<IRefreshTokenFamilyRepository, RefreshTokenFamilyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        var roleOptionsSection = configuration.GetSection(nameof(RoleOptions));
        services.Configure<RoleOptions>(x =>
        {
            x.DefaultUserRoles = roleOptionsSection
                .GetValue(nameof(RoleOptions.DefaultUserRoles), new List<string>())!;
        });
    }

    private static void AddAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMemoryCache();

        // Register the custom policy provider
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        // Permission based authorization setup
        services.AddTransient<IAuthorizationHandler, PermissionHandler>();
        services.AddTransient<IFeatureAccessProvider, InternalFeatureAccessProvider>();

        // Client credential flow authorization
        services.AddTransient<IAuthorizationHandler, ClientHandler>();

        // Authorized user setup
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IClientAccessor, ClientAccessor>();
        services.AddHttpContextAccessor();

        // JWT setup
        var securityOptionsSection = configuration.GetSection(nameof(SecurityOptions));
        services.Configure<SecurityOptions>(x =>
        {
            x.SecretKey = securityOptionsSection[nameof(SecurityOptions.SecretKey)] 
                          ?? throw new ArgumentNullException(null, "ServiceSecret key is not specified");

            x.AccessTokenLifetimeInMinutes = securityOptionsSection
                .GetValue(nameof(SecurityOptions.AccessTokenLifetimeInMinutes), 15);

            x.RefreshTokenLifetimeInMinutes = securityOptionsSection
                .GetValue(nameof(SecurityOptions.RefreshTokenLifetimeInMinutes), 180);

            x.ServiceAccessTokenLifeTimeInDays = securityOptionsSection
                .GetValue(nameof(SecurityOptions.ServiceAccessTokenLifeTimeInDays), 1);
        });

        services.Configure<PasswordOptions>(x =>
        {
            x.Salt = securityOptionsSection.GetSection(nameof(PasswordOptions))[nameof(PasswordOptions.Salt)]
                     ?? throw new ArgumentNullException(null, "Password salt is not specified");
        });
    }

    private static async Task RunApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();
        var appName = builder.Configuration["ServiceName"]
                      ?? throw new ArgumentNullException(null, "Service name is not specified");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DataBaseContext>();
        await context.Database.EnsureCreatedAsync();

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