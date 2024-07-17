using NLog;
using NLog.Web;
using Packages.Application.Consul;
using System.Reflection;
using MSG.Security.Authentication.Integration;
using MSG.Security.Authorization.Integration;

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

    private static void ConfigureDI(IServiceCollection services, ConfigurationManager configuration)
    {

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