using MediatR;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.InvalidateRefreshToken;

/// <summary>
/// Invalidate refresh token command
/// </summary>
public class InvalidateRefreshTokenCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; }

    public InvalidateRefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
