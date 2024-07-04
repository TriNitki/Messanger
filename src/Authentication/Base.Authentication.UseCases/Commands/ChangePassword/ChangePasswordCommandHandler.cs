using Base.Authentication.Contracts;
using Base.Authentication.Core.Options;
using Base.Authentication.Core.Services;
using Base.Authentication.UseCases.Abstractions;
using Base.Authentication.UseCases.Commands.Login;
using MediatR;
using Microsoft.Extensions.Options;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.ChangePassword;

/// <summary>
/// Change password command handler
/// </summary>
public class ChangePasswordCommandHandler : LoginBaseHandler, IRequestHandler<ChangePasswordCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly PasswordOptions _passwordOptions;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IOptions<PasswordOptions> passwordOptions,
        ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordOptions = passwordOptions.Value ?? throw new ArgumentNullException(nameof(passwordOptions));
    }

    public async Task<Result<Tokens>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.ResolveAsync(
            request.Login,
            CryptographyService.HashPassword(request.Password, _passwordOptions.Salt));

        if (user is null)
            return Result<Tokens>.Invalid("Invalid password or login");

        user.SetPassword(request.NewPassword, _passwordOptions);
        await _userRepository.UpdateAsync(user);
        await _refreshTokenRepository.DeactivateAllTokensAsync(user.Id);

        return await Handle(user);
    }
}
