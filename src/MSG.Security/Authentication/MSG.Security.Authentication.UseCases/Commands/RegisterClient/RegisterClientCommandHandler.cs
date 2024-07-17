using MediatR;
using Microsoft.Extensions.Options;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.Core.Options;
using MSG.Security.Authentication.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.RegisterClient;

/// <summary>
/// Command handler to register a new client
/// </summary>
public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, Result<string>>
{
    private readonly ITokenService _tokenService;
    private readonly IClientRepository _clientRepository;
    private readonly PasswordOptions _passwordOptions;


    public RegisterClientCommandHandler(
        ITokenService tokenService, 
        IClientRepository clientRepository, 
        IOptions<PasswordOptions> passwordOptions)
    {
        _tokenService = tokenService;
        _clientRepository = clientRepository;
        _passwordOptions = passwordOptions.Value;
    }

    public async Task<Result<string>> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        if (await _clientRepository.ResolveAsync(request.Name) is not null)
            return Result<string>.Conflict("Client with this name already exists");

        var client = await _clientRepository.CreateAsync(
            new AuthClient(request.Name, request.Secret, _passwordOptions));

        var accessToken = await _tokenService.GenerateAccessToken(client);

        return Result<string>.Success(accessToken);
    }
}