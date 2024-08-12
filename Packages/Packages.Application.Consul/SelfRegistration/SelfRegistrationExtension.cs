using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Packages.Application.Consul.SelfRegistration;

/// <summary>
/// DI extension for service registration in Consul and Fabio
/// </summary>
internal static class SelfRegistrationExtension
{
    /// <summary>
    /// Service registration with Consul and Fabio
    /// </summary>
    /// <param name="services"> Service Collection </param>
    /// <param name="configuration"> Application Configuration </param>
    /// <returns> Service Collection </returns>
    internal static IServiceCollection AddSelfRegistrationWithConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConsul(configuration);
        services.AddHealthChecks();

        services.AddOptions<SelfRegistrationOptions>().Bind(configuration).PostConfigure<IServiceProvider>(static (x, sp) =>
        {
            if (!string.IsNullOrEmpty(x.ApplicationUrl))
            {
                return;
            }

            var configuration = sp.GetRequiredService<IConfiguration>();
            var aspUrls = configuration["ASPNETCORE_URLS"];

            if (string.IsNullOrEmpty(aspUrls))
            {
                var log = sp.GetRequiredService<ILogger<SelfRegistrationService>>();
                log.LogInformation("No fallback for ApplicationUrl (missing ASPNETCORE_URLS)");
                return;
            }

            x.ApplicationUrl = aspUrls.IndexOf(';') is var i and not -1 ? aspUrls[..i] : aspUrls;
        });

        services.AddHostedService<SelfRegistrationService>();
        return services;
    }
}