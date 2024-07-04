using Base.Authentication.Contracts;
using Refit;

namespace Base.Authentication.Clients;

/// <summary>
/// Authorization client
/// </summary>
public interface IAuthorizationClient
{
    /// <summary>
    /// Authorize
    /// </summary>
    /// <param name="request"> Login request model </param>
    /// <returns> Authorization tokens </returns>
    [Post("/api/auth/login")]
    public Task<Tokens> Login(LoginRequest request);

    /// <summary>
    /// Refresh tokens
    /// </summary>
    /// <param name="request"> Refresh tokens model </param>
    /// <returns> Authorization tokens </returns>
    [Post("/api/auth/refresh")]
    public Task<Tokens> RefreshTokens(RefreshTokenRequest request);
}
