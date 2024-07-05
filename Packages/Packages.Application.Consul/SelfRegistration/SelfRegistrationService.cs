using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Microsoft.Extensions.Hosting;

namespace Packages.Application.Consul.SelfRegistration;

internal sealed class SelfRegistrationService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly SelfRegistrationOptions _options;
    private readonly ILogger<SelfRegistrationService> _logger;
    private string? _registrationId;

    public SelfRegistrationService(IConsulClient consulClient, IOptions<SelfRegistrationOptions> options, ILogger<SelfRegistrationService> logger)
    {
        _consulClient = consulClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var registration = CreateAgentServiceRegistration(_options);
        var id = registration.ID;

        _logger.LogTrace("Service registration [id: {ServiceName}] in Consul...", _options.ServiceName);
        var response = await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

        if (response.IsOk())
        {
            _registrationId = id;
            _logger.LogTrace("[id: {ServiceName}] service is registered in Consul.", _options.ServiceName);
            return;
        }

        _logger.LogError("[id: {RegistrationID}] service registration error in Consul.", id);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_registrationId))
            return;

        _logger.LogTrace("[id: {RegistrationId}] service deregistration from Consul...", _registrationId);

        try
        {
            await _consulClient.Agent.ServiceDeregister(_registrationId, cancellationToken);
            _logger.LogTrace("[id: {RegistrationId}] service is deregistered from Consul.", _registrationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[id: {RegistrationId}] service deregistration error from Consul.", _registrationId);
        }
    }

    private static AgentServiceRegistration CreateAgentServiceRegistration(SelfRegistrationOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ServiceName))
            throw new ConsulConfigurationException(
                "ServiceName is not specified in the configuration. Specify the option in appsettings.json or an environment variable");

        if (options.ApplicationUrl is { } appUrl && (appUrl.Contains('*') || appUrl.Contains('+')))
        {
            var hostName = Dns.GetHostEntry("").HostName;
            options.ApplicationUrl = appUrl.Replace("*", hostName).Replace("+", hostName);
        }

        if (!Uri.TryCreate(options.ApplicationUrl, UriKind.Absolute, out var uri))
            throw new ConsulConfigurationException(
                "Configuration does not specify the ApplicationUrl on which the application is hosted. Specify the option in appsettings.json or an environment variable");

        var registration = new AgentServiceRegistration
        {
            ID = $"{uri.Authority}-{DateTime.UtcNow.Ticks}",
            Name = options.ServiceName,

            Address = uri.Host,
            Port = uri.Port,
            // интеграция с fabio 
            Tags =
            [
                $"urlprefix-/{options.ServiceName} strip=/{options.ServiceName}"
            ],
            Check = new AgentServiceCheck
            {
                HTTP = $"{uri.Scheme}://{uri.Authority}/health",
                DeregisterCriticalServiceAfter = TimeSpan.FromHours(2),
                Timeout = TimeSpan.FromSeconds(options.PingTimeoutInSeconds ?? 3),
                Interval = TimeSpan.FromSeconds(options.PingIntervalInSeconds ?? 30)
            }
        };
        return registration;
    }
}

internal static class WriteResultStatusCodeExtensions
{
    public static bool IsOk(this WriteResult? result)
    {
        if (result is null)
            return false;
        return (int)result.StatusCode is >= 200 and < 300;
    }
}