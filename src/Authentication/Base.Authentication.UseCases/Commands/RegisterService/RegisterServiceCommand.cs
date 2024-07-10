using MediatR;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.RegisterService;

/// <summary>
/// Command to register a new service
/// </summary>
public class RegisterServiceCommand : IRequest<Result<string>>
{
    /// <summary>
    /// Name of the service
    /// </summary>
    public string ServiceName { get; }

    public RegisterServiceCommand(string serviceName)
    {
        ServiceName = serviceName;
    }
}