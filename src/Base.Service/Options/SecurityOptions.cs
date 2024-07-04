namespace Base.Service.Options;

/// <summary>
/// Security options
/// </summary>
public class SecurityOptions
{
    /// <summary>
    /// Secret key for JWT token generation
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Access token lifetime in minutes
    /// </summary>
    public int AccessTokenLifetimeInMinutes { get; set; }

    /// <summary>
    /// Refresh token lifetime in minutes
    /// </summary>
    public int RefreshTokenLifetimeInMinutes { get; set; }
}