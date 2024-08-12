using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Packages.Application.Consul.SelfRegistration;

namespace Packages.Application.Consul;

public static class ConsulExtensionsForDi
{
    /// <summary>
    /// <list type="bullet">
    /// <item>
    /// Adds configurations from Consul K/V (ключи <c>"cfg/app-base-settings"</c>,
    /// <c>"{ServiceName}/cfg"</c> and listed in <c>{AdditionalCfgNodes}</c>).
    /// Configuration derived from environment variables and from startup arguments have a higher priority.
    /// </item>
    /// <item>Registers the application in Consul Catalog and Fabio with the name <c>{ServiceName}</c> and the address <c>{ApplicationUrl}</c>.</item>
    /// <item>Adds the configured <see cref="IConsulClient"/> into a DI container.</item>
    /// </list>
    /// </summary>
    /// <remarks>Consul address from <c>{ConsulUri}</c>.</remarks>
    public static void AddConsulIntegration(this IServiceCollection services, ConfigurationManager configuration)
    {
        ConfigureAppConfiguration(configuration, configuration);
        ConfigureServices(services, configuration);
    }

    /// <summary>
    /// Adds <see cref="IConsulClient"/> to <paramref name="services"/>
    /// </summary>
    /// <param name="services">Service Collection</param>
    /// <param name="configuration"> Configuration </param>
    /// <returns>Service Collection</returns>
    internal static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        var consulUri = configuration["ConsulUri"];

        if (string.IsNullOrWhiteSpace(consulUri))
            throw new ConsulConfigurationException("Consul connection string not set");

        var uri = new Uri(consulUri);

        services.TryAddSingleton<IConsulClient>(_ => 
            new ConsulClient(consulConfig => { consulConfig.Address = uri; }));
        return services;
    }

    private static void ConfigureAppConfiguration(
        IConfigurationBuilder builder,
        IConfiguration config)
    {
        string[] cfgNodes =
        [
            "cfg/app-base-settings", $"{config["ServiceName"]}/cfg"
        ];

        if (config.GetSection("AdditionalCfgNodes").Get<string[]>() is { } arr)
        {
            cfgNodes = cfgNodes.Concat(arr).ToArray();
        }

        if (config.GetValue<bool>("DebugOptions:DisableConsulBaseConfig"))
        {
            cfgNodes = cfgNodes.Skip(1).ToArray();
        }

        builder.AddConsulBeforeEnv(config["ConsulUri"]!, cfgNodes);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSelfRegistrationWithConsul(config);
    }
}