using System.Security.Claims;

namespace Base.Authentication.Core.Abstractions;

/// <summary>
/// Base authentication client
/// </summary>
public abstract class BaseAuthClient
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Whether the user is blocked
    /// </summary>
    public bool IsBlocked { get; set; }

    /// <summary>
    /// User roles
    /// </summary>
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// Get user claims
    /// </summary>
    /// <returns> Array of claims </returns>
    public virtual Claim[] GetClaims()
    {
        var claims = new List<Claim>(Roles.Length + 1)
        {
            new(ClaimTypes.Name, Login)
        };

        foreach (var role in Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims.ToArray();
    }
}