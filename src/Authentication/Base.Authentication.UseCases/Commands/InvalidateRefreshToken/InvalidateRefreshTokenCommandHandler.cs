using Base.Authentication.UseCases.Abstractions;
using MediatR;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.InvalidateRefreshToken;

/// <summary>
/// Invalidate refresh token command handler
/// </summary>
public class InvalidateRefreshTokenCommandHandler : IRequestHandler<InvalidateRefreshTokenCommand, Result<Unit>>
{
    private readonly ITokenService _tokenService;

    public InvalidateRefreshTokenCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
    }

    public async Task<Result<Unit>> Handle(InvalidateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        await _tokenService.DeactivateRefreshToken(request.RefreshToken);
        return Result<Unit>.NoContent();
    }
}
