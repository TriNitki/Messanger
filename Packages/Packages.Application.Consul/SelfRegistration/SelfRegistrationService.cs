using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;

namespace Packages.Application.Consul.SelfRegistration;

internal sealed class SelfRegistrationService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly SelfRegistrationOptions _options;
    private readonly ILogger<SelfRegistrationService> _logger;
    private string? _registrationId;

    public SelfRegistrationService(
        IConsulClient consulClient,
        IOptions<SelfRegistrationOptions> options,
        ILogger<SelfRegistrationService> logger)
    {
        _consulClient = consulClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var registration = SelfRegistrationService.CreateAgentServiceRegistration(_options);
        var id = registration.ID;
        _logger.LogTrace("Service registration [id: {ServiceName}] in Consul...", _options.ServiceName);
        var response = await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

        if (response.IsOk())
        {
            _registrationId = id;
            _logger.LogTrace("[id: {ServiceName}] service is registered in Consul.", _options.ServiceName);
        }
        else
        {
            _logger.LogError("[id: {RegistrationID}] service registration error in Consul.", id);
        }
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
        var str = !string.IsNullOrWhiteSpace(options.ServiceName) 
            ? options.ApplicationUrl 
            : throw new ConsulConfigurationException(
                "ServiceName is not set in the configuration. Specify the option in appsettings.json or an environment variable");

        if (str != null && (str.Contains('*') || str.Contains('+')))
        {
            var hostName = Dns.GetHostEntry("").HostName;
            options.ApplicationUrl = str.Replace("*", hostName).Replace("+", hostName);
        }

        if (!Uri.TryCreate(options.ApplicationUrl, UriKind.Absolute, out var result))
            throw new ConsulConfigurationException(
                "Configuration does not specify the ApplicationUrl on which the application is hosted. Specify the option in appsettings.json or an environment variable");
            
        var serviceRegistration1 = new AgentServiceRegistration();
        var serviceRegistration2 = serviceRegistration1 ?? throw new ArgumentNullException(nameof(serviceRegistration1));
            
        var interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
            
        interpolatedStringHandler.AppendFormatted(result.Authority);
        interpolatedStringHandler.AppendLiteral("-");
        interpolatedStringHandler.AppendFormatted(DateTime.UtcNow.Ticks);
            
        var stringAndClear = interpolatedStringHandler.ToStringAndClear();
            
        serviceRegistration2.ID = stringAndClear;

        serviceRegistration1.Name = options.ServiceName;
        serviceRegistration1.Address = result.Host;
        serviceRegistration1.Port = result.Port;
        serviceRegistration1.Tags =
        [
            "urlprefix-/" + options.ServiceName + " strip=/" + options.ServiceName
        ];
            
        var serviceRegistration3 = serviceRegistration1 ?? throw new ArgumentNullException(nameof(serviceRegistration1));
        var agentServiceCheck = new AgentServiceCheck
        {
            HTTP = result.Scheme + "://" + result.Authority + "/health",
            DeregisterCriticalServiceAfter = TimeSpan.FromHours(2.0)
        };

        var nullable = options.PingTimeoutInSeconds;
        agentServiceCheck.Timeout = TimeSpan.FromSeconds(nullable ?? 3.0);

        nullable = options.PingIntervalInSeconds;
        agentServiceCheck.Interval = TimeSpan.FromSeconds(nullable ?? 30.0);
            
        serviceRegistration3.Check = agentServiceCheck;
            
        return serviceRegistration1;
    }
}