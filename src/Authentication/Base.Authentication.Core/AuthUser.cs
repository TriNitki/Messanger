using Base.Authentication.Core.Options;
using Base.Authentication.Core.Services;
using System.Security.Claims;

namespace Base.Authentication.Core;

/// <summary>
/// Authenticated user
/// </summary>
public class AuthUser
{
    /// <summary>
    /// Unique id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; } = string.Empty;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// Whether the user is blocked
    /// </summary>
    public bool IsBlocked { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// User roles
    /// </summary>
    public string[] Roles { get; set; } = Array.Empty<string>();

    /// <summary>
    /// New user constructor
    /// </summary>
    /// <param name="login"> Login </param>
    /// <param name="password"> Password </param>
    /// <param name="passwordOptions"> Password options </param>
    /// <param name="roles"> Roles </param>
    public AuthUser(string login, string password, PasswordOptions passwordOptions, string[] roles)
    {
        Login = login;
        Roles = roles;
        Id = Guid.NewGuid();

        SetPassword(password, passwordOptions);
    }

    /// <summary>
    /// Existing user constructor
    /// </summary>
    /// <param name="id"> Id </param>
    /// <param name="login"> Login </param>
    /// <param name="passwordHash"> Hashed password </param>
    /// <param name="isBlocked"> Whether the user is blocked </param>
    /// <param name="email"> Email </param>
    /// <param name="roles"> Roles </param>
    public AuthUser(Guid id, string login, string passwordHash, bool isBlocked, string? email, string[] roles)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        IsBlocked = isBlocked;
        Email = email;
        Roles = roles;
    }

    /// <summary>
    /// Set password
    /// </summary>
    /// <param name="password"> Password </param>
    /// <param name="passwordOptions"> Password options </param>
    public void SetPassword(string password, PasswordOptions passwordOptions) 
    {
        PasswordHash = CryptographyService.HashPassword(password, passwordOptions.Salt);
    }

    /// <summary>
    /// Get user claims
    /// </summary>
    /// <returns> Array of claims </returns>
    public Claim[] GetClaims()
    {
        var claims = new List<Claim>(Roles.Length + 2)
        {
            new(ClaimTypes.NameIdentifier, Id.ToString()),
            new(ClaimTypes.Name, Login)
        };

        foreach (var role in Roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        return claims.ToArray();
    }
}
