using Base.Authentication.Core;

namespace Base.Authentication.UseCases.Abstractions;

/// <summary>
/// A service for working with tokens
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generate access token
    /// </summary>
    /// <param name="user"> Authenticated user </param>
    /// <returns> Access token </returns>
    Task<string> GenerateAccessToken(AuthUser user);

    /// <summary>
    /// Generate refresh token
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <param name="tokenFamilyId"> Token family id </param>
    /// <returns> Refresh token </returns>
    Task<RefreshToken> GenerateRefreshToken(Guid userId, Guid tokenFamilyId);

    /// <summary>
    /// Use refresh token
    /// </summary>
    /// <param name="token"> Refresh token </param>
    /// <returns> Deactivated refresh token </returns>
    Task<RefreshToken?> UseRefreshToken(string token);
}
