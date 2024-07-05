namespace Packages.Application.Consul.SelfRegistration;

/// <summary>
/// Parameters of service registration in Consul
/// </summary>
internal class SelfRegistrationOptions
{
    /// <summary>
    /// Service name
    /// </summary>
    public string? ServiceName { get; set; }

    /// <summary>
    /// Service address
    /// </summary>
    public string? ApplicationUrl { get; set; }

    /// <summary>
    /// Ping timeout
    /// </summary>
    public double? PingTimeoutInSeconds { get; set; }

    /// <summary>
    /// Ping interval
    /// </summary>
    public double? PingIntervalInSeconds { get; set; }
}