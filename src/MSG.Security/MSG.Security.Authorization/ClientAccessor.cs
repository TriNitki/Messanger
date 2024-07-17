using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MSG.Security.Authorization;

/// <summary>
/// <see cref="IClientAccessor"/> implementation
/// </summary>
public class ClientAccessor : IClientAccessor
{
    private readonly ClaimsPrincipal _user;

    /// <inheritdoc/>
    public string Id
    {
        get
        {
            var clientClaim = _user.Claims.FirstOrDefault(x => x!.Type == "client_id", null);
            return clientClaim is null || string.IsNullOrEmpty(clientClaim.Value)
                ? string.Empty
                : clientClaim.Value;
        }
    }

    public ClientAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext.User;
    }
}