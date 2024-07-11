using MediatR;
using Microsoft.Extensions.Options;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.Core.Options;
using MSG.Security.Authentication.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.RegisterService;

public class RegisterServiceCommandHandler : IRequestHandler<RegisterServiceCommand, Result<string>>
{
    private readonly ITokenService _tokenService;
    private readonly string[] _defaultServiceRoles;

    public RegisterServiceCommandHandler(ITokenService tokenService, IOptions<RoleOptions> roles)
    {
        _tokenService = tokenService;
        _defaultServiceRoles = roles.Value.DefaultServiceRoles.ToArray();
    }

    public async Task<Result<string>> Handle(RegisterServiceCommand request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenService.GenerateServiceAccessToken(
            new AuthService(request.ServiceName, _defaultServiceRoles));

        return Result<string>.Success(accessToken);
    }
}