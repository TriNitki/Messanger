namespace Packages.Application.Consul.SelfRegistration;

internal class SelfRegistrationOptions
{
    public string? ServiceName { get; set; }

    public string? ApplicationUrl { get; set; }

    public double? PingTimeoutInSeconds { get; set; }

    public double? PingIntervalInSeconds { get; set; }
}