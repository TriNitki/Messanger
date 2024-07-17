using MediatR;
using Microsoft.Extensions.Options;
using MSG.Security.Authentication.Core.Options;
using MSG.Security.Authentication.Core.Services;
using MSG.Security.Authentication.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.LoginClient;

/// <summary>
/// Command handler to get access token for existing client
/// </summary>
public class LoginClientCommandHandler : IRequestHandler<LoginClientCommand, Result<string>>
{
    private readonly IClientRepository _clientRepository;
    private readonly ITokenService _tokenService;
    private readonly string _salt;

    public LoginClientCommandHandler(
        IClientRepository clientRepository, 
        IOptions<PasswordOptions> passwordOptions, 
        ITokenService tokenService)
    {
        _clientRepository = clientRepository;
        _tokenService = tokenService;
        _salt = passwordOptions.Value.Salt;
    }

    public async Task<Result<string>> Handle(LoginClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.ResolveAsync(
            request.Name,
            CryptographyService.HashPassword(request.Secret, _salt));

        if(client is null)
            return Result<string>.Invalid("Invalid secret or name");

        var accessToken = await _tokenService.GenerateAccessToken(client);

        return Result<string>.Success(accessToken);
    }
}