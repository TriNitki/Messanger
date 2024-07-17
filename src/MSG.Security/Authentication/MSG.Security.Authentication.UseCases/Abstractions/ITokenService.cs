using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.Core.Abstractions;

namespace MSG.Security.Authentication.UseCases.Abstractions;

/// <summary>
/// Service for working with tokens
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generate new access token
    /// </summary>
    /// <param name="customer"> Authenticated customer </param>
    /// <returns> Access token </returns>
    Task<string> GenerateAccessToken(IAuthenticatedCustomer customer);

    /// <summary>
    /// Generate new refresh token
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <param name="tokenFamilyId"> Token family id </param>
    /// <returns> Refresh token </returns>
    Task<RefreshToken> GenerateRefreshToken(Guid userId, Guid tokenFamilyId);

    /// <summary>
    /// Mark refresh token as used
    /// </summary>
    /// <param name="token"> Refresh token </param>
    /// <returns> Used refresh token </returns>
    Task<RefreshToken?> UseRefreshToken(string token);
}
