using Base.Authentication.Contracts;
using Base.Authentication.UseCases.Abstractions;
using Base.Authentication.UseCases.Commands.Login;
using MediatR;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : LoginBaseHandler, IRequestHandler<RefreshTokensCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;

    public RefreshTokensCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService) : base(tokenService)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Tokens>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenService.DeactivateRefreshToken(request.RefreshToken);

        if (refreshToken is null)
            return Result<Tokens>.Invalid("Refresh token is incorrect");

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId) ?? throw new NullReferenceException();

        if (user.IsBlocked)
            return Result<Tokens>.Unauthorized("User is blocked");

        return await Handle(user);
    }
}
