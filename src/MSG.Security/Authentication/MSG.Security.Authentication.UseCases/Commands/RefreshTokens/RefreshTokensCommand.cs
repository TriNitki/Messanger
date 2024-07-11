using MediatR;
using MSG.Security.Authentication.Contracts;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.RefreshTokens;

/// <summary>
/// Refresh a pair of tokens command
/// </summary>
public class RefreshTokensCommand : IRequest<Result<Tokens>>
{
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; }

    public RefreshTokensCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
