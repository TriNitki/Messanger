using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Packages.Application.Consul.SelfRegistration;

namespace Packages.Application.Consul;

public static class ConsulExtensionsForDi
{
    public static void AddConsulIntegration(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        ConfigureAppConfiguration(configuration, configuration);
        ConfigureServices(services, configuration);
    }

    internal static IServiceCollection AddConsul(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var uriString = configuration["ConsulUri"];
        var uri = !string.IsNullOrWhiteSpace(uriString) 
            ? new Uri(uriString) 
            : throw new ConsulConfigurationException("Consul connection string not set");
        
        services.TryAddSingleton(_ => (IConsulClient)new ConsulClient((Action<ConsulClientConfiguration>)(consulConfig => consulConfig.Address = uri)));
        
        return services;
    }

    private static void ConfigureAppConfiguration(
        IConfigurationBuilder builder,
        IConfiguration config)
    {
        string[] strArray =
        [
            "cfg/app-base-settings",
            config["ServiceName"] + "/cfg"
        ];

        string[]? second = config.GetSection("AdditionalCfgNodes").Get<string[]>();

        if (second != null)
            strArray = strArray.Concat(second).ToArray();

        if (config.GetValue<bool>("DebugOptions:DisableConsulBaseConfig"))
            strArray = strArray.Skip(1).ToArray();

        builder.AddConsulBeforeEnv(config["ConsulUri"]!, strArray);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services.AddSelfRegistrationWithConsul(config);
    }
}