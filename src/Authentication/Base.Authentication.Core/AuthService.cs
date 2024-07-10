using Base.Authentication.Core.Abstractions;

namespace Base.Authentication.Core;

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