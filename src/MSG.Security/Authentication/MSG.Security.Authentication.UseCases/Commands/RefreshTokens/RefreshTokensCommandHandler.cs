using MediatR;
using MSG.Security.Authentication.Contracts;
using MSG.Security.Authentication.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.RefreshTokens;

public class RefreshTokensCommandHandler : IRequestHandler<RefreshTokensCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokensCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<Tokens>> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenService.UseRefreshToken(request.RefreshToken);

        if (refreshToken is null)
            return Result<Tokens>.Invalid("Refresh token is incorrect");

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId) ?? throw new NullReferenceException();

        if (user.IsBlocked)
            return Result<Tokens>.Unauthorized("User is blocked");

        var newAccessToken = await _tokenService.GenerateAccessToken(user);
        var newRefreshToken = await _tokenService.GenerateRefreshToken(user.Id, refreshToken.FamilyId);

        return Result<Tokens>.Success(
            new Tokens(newAccessToken, newRefreshToken.Content, newRefreshToken.Expiration));
    }
}
