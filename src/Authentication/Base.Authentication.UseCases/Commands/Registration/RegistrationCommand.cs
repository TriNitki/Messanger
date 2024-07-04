using Base.Authentication.Contracts;
using MediatR;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.Registration;

/// <summary>
/// Registration command
/// </summary>
public class RegistrationCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; }

    public RegistrationCommand(string login, string password)
    {
        Login = login;
        Password = password;
    }
}
