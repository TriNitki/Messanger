using MediatR;
using MSG.Security.Authentication.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.InvalidateRefreshToken;

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
        await _tokenService.UseRefreshToken(request.RefreshToken);
        return Result<Unit>.NoContent();
    }
}
