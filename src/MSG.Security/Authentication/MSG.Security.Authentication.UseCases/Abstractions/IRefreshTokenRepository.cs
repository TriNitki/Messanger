using MSG.Security.Authentication.Core;

namespace MSG.Security.Authentication.UseCases.Abstractions;

/// <summary>
/// Repository for accessing refresh tokens
/// </summary>
public interface IRefreshTokenRepository
{
    /// <summary>
    /// Get the entity by token
    /// </summary>
    /// <param name="token"> Token </param>
    /// <returns> Token entity </returns>
    Task<RefreshToken?> GetByTokenAsync(string token);

    /// <summary>
    /// Create token
    /// </summary>
    /// <param name="refreshToken"> Token entity </param>
    Task CreateAsync(RefreshToken refreshToken);

    /// <summary>
    /// Deactivate token
    /// </summary>
    /// <param name="token"> Token entity </param>
    Task DeactivateAsync(RefreshToken token);

    /// <summary>
    /// Mark all user tokens as used
    /// </summary>
    /// <param name="userId"> User id </param>
    Task DeactivateAllTokensAsync(Guid userId);
}
