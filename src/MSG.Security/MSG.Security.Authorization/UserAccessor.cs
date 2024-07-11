using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MSG.Security.Authorization;

/// <summary>
/// <see cref="IUserAccessor"/> implementation
/// </summary>
public class UserAccessor : IUserAccessor
{
    private readonly ClaimsPrincipal _user;

    /// <inheritdoc/>
    public string Login {
        get
        {
            var login = _user.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            return string.IsNullOrEmpty(login) 
                ? string.Empty 
                : login;
        }
    }

    /// <inheritdoc/>
    public Guid Id {
        get
        {
            if (Guid.TryParse(_user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value, out var id))
                return id;

            return Guid.Empty;
        }

    }

    /// <inheritdoc/>
    public string[] Roles {
        get
        {
            return _user.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();
        }
    }

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext.User;
    }
}
