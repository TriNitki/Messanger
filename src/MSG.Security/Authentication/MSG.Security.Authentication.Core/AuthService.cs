using MSG.Security.Authentication.Core.Abstractions;

namespace MSG.Security.Authentication.Core;

public class AuthService : BaseAuthClient
{
    public AuthService(string login)
    {
        Login = login;
        Roles = ["Service"];
        IsBlocked = false;
    }

    public AuthService(string login, string[] roles)
    {
        Login = login;
        Roles = roles;
        IsBlocked = false;
    }
}