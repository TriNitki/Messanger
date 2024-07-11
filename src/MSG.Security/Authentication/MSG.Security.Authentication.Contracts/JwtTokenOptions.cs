namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// Options for jwt token
/// </summary>
public class JwtTokenOptions
{
    /// <summary>
    /// Access token expiration time in minutes
    /// </summary>
    public int AccessTokenExpirationInMinutes { get; set; }
}
