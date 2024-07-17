using MediatR;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.LoginClient;

/// <summary>
/// Command to get access token for existing client
/// </summary>
public class LoginClientCommand : IRequest<Result<string>>
{
    /// <summary>
    /// Client name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Client secret
    /// </summary>
    public string Secret { get; }

    public LoginClientCommand(string name, string secret)
    {
        Name = name;
        Secret = secret;
    }
}