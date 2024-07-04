using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Packages.Application.Consul.SelfRegistration;

internal static class SelfRegistrationExtension
{
    internal static IServiceCollection AddSelfRegistrationWithConsul(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddConsul(configuration);
        services.AddHealthChecks();
        services.AddOptions<SelfRegistrationOptions>()
            .Bind(configuration)
            .PostConfigure((Action<SelfRegistrationOptions, IServiceProvider>)((x, sp) =>
            {
                if (!string.IsNullOrEmpty(x.ApplicationUrl))
                    return;

                var str1 = sp.GetRequiredService<IConfiguration>()["urls"];

                if (string.IsNullOrEmpty(str1))
                {
                    sp.GetRequiredService<ILogger<SelfRegistrationService>>().LogInformation("No fallback for ApplicationUrl (missing ASPNETCORE_URLS)");
                }
                else
                {
                    var registrationOptions = x;
                    var length = str1.IndexOf(';');
                    var str2 = length != -1 ? str1.Substring(0, length) : str1;
                    registrationOptions.ApplicationUrl = str2;
                }
            }));

        services.AddHostedService<SelfRegistrationService>();
        return services;
    }
}