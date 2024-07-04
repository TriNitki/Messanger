using Base.Authentication.Contracts;
using MediatR;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.ChangePassword;

/// <summary>
/// Change password command
/// </summary>
public class ChangePasswordCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; }

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; }

    public ChangePasswordCommand(string login, string password, string newPassword)
    {
        Login = login;
        Password = password;
        NewPassword = newPassword;
    }
}
