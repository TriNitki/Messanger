using MSG.Security.Authentication.Contracts;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.Login;

/// <summary>
/// Handler for login based handlers
/// </summary>
public abstract class LoginBaseHandler
{
    protected readonly ITokenService _tokenService;

    public LoginBaseHandler(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));   
    }

    /// <summary>
    /// User authorization
    /// </summary>
    /// <param name="user"> User </param>
    /// <returns> Authorization result </returns>
    public async Task<Result<Tokens>> Handle(AuthUser user)
    {
        string accessToken = await _tokenService.GenerateAccessToken(user);
        RefreshToken refreshToken = await _tokenService.GenerateRefreshToken(user.Id, Guid.NewGuid());

        return Result<Tokens>.Success(
            new Tokens(accessToken, refreshToken.Content, refreshToken.Expiration));
    } 
}
