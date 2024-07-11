using MediatR;
using MSG.Security.Authentication.Contracts;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.Login;

/// <summary>
/// Login command
/// </summary>
public class LoginCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; }

    public LoginCommand(string login, string password)
    {
        Login = login;
        Password = password;
    }
}
