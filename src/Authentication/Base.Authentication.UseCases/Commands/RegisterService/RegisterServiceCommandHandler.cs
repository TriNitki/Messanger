using Base.Authentication.Core;
using Base.Authentication.Core.Options;
using Base.Authentication.UseCases.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.RegisterService;

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