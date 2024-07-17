using System.Security.Claims;
using MSG.Security.Authentication.Core.Abstractions;
using MSG.Security.Authentication.Core.Options;
using MSG.Security.Authentication.Core.Services;

namespace MSG.Security.Authentication.Core;

/// <summary>
/// Authenticated client
/// </summary>
public class AuthClient : IAuthenticatedCustomer
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Hashed client secret
    /// </summary>
    public string HashedSecret { get; private set; } = string.Empty;

    public AuthClient(string name, string secret, PasswordOptions passwordOptions)
    {
        Name = name;
        SetSecret(secret, passwordOptions);
    }

    public AuthClient(string name, string hashedSecret)
    {
        Name = name;
        HashedSecret = hashedSecret;
    }

    /// <summary>
    /// Set secret
    /// </summary>
    /// <param name="secret"> Client secret </param>
    /// <param name="passwordOptions"> Password options </param>
    public void SetSecret(string secret, PasswordOptions passwordOptions)
    {
        HashedSecret = CryptographyService.HashPassword(secret, passwordOptions.Salt);
    }

    /// <summary>
    /// Get client claims
    /// </summary>
    /// <returns> Array of claims </returns>
    public Claim[] GetClaims()
    {
        var claims = new List<Claim>(1)
        {
            new("client_id", Name),
        };

        return claims.ToArray();
    }
}