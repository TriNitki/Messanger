using NLog;
using NLog.Web;
using Packages.Application.Consul;
using System.Reflection;
using MSG.Messenger.DataAccess;
using MSG.Messenger.DataAccess.Repositories;
using MSG.Messenger.Observer.EventHandlers;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Abstractions;
using MSG.Messenger.UseCases.Commands.CreateGroupChat;
using MSG.Messenger.UseCases.Notifications;
using MSG.Security.Authentication.Integration;
using MSG.Security.Authorization.Integration;
using Packages.Application.Data.DI;
using MSG.Security.Authorization;

namespace MSG.Messenger.Service;

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

    /// <summary>
    /// Configure application
    /// </summary>
    /// <param name="args"> List of args </param>
    /// <returns> Application builder </returns>
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

        services.AddJwtBearerAuthentication(cfg["SecretKey"]!);

        services.AddPermissionBasedAuthorization(cfg);

        var basePath = AppContext.BaseDirectory;
        var xmlFilePath = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        services.AddSwagger(Path.Combine(basePath, xmlFilePath));

        ConfigureDI(services, cfg);

        return builder;
    }

    /// <summary>
    /// Configure DI
    /// </summary>
    /// <param name="services"> Service collection </param>
    /// <param name="configuration"> Configuration </param>
    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAutoMapper(typeof(DbMappingProfile).Assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([
            typeof(CreateGroupChatCommand).Assembly,
            typeof(SendMessageEventHandler).Assembly
        ]));
        services.AddDataContext<DataBaseContext>(configuration);
        services.AddSignalR();

        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
        services.AddScoped<IChatMessageRepository, ChatMessageRepository>();

        services.AddScoped<UserAccessor>();
    }

    /// <summary>
    /// Run application
    /// </summary>
    /// <param name="builder"> Application builder </param>
    /// <exception cref="ArgumentNullException"> Service name is not specified </exception>
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

        app.MapHub<MessengerHub>("hubs/messenger");
        app.MapControllers();

        await app.RunAsync();
    }
}