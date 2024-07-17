using MediatR;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.RegisterClient;

/// <summary>
/// Command to register a new client
/// </summary>
public class RegisterClientCommand : IRequest<Result<string>>
{
    /// <summary>
    /// Client name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Client secret
    /// </summary>
    public string Secret { get; }

    public RegisterClientCommand(string name, string secret)
    {
        Name = name;
        Secret = secret;
    }
}