using MSG.Security.Authentication.Contracts;
using Refit;

namespace MSG.Security.Authentication.Clients.Abstractions;

/// <summary>
/// Authorization client
/// </summary>
public interface IAuthorizationClient
{
    /// <summary>
    /// Authorize
    /// </summary>
    /// <param name="clientRequest"> Login clientRequest model </param>
    /// <returns> Authorization tokens </returns>
    [Post("/api/auth/login/client")]
    public Task<ApiResponse<string?>> Login(LoginClientRequest clientRequest);
}
