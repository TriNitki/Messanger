namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// User options
/// </summary>
public class ClientOptions
{
    /// <summary>
    /// Service name
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Service secret
    /// </summary>
    public string ServiceSecret { get; set; } = string.Empty;
}
