using System.ComponentModel.DataAnnotations;

namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// User
/// </summary>
public class User
{
    /// <summary>
    /// User's id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's login
    /// </summary>
    [MaxLength(64)]
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// User's hashed password
    /// </summary>
    [MinLength(64), MaxLength(64)]
    public string HashedPassword { get; set; } = string.Empty;

    /// <summary>
    /// Whether user is blocked
    /// </summary>
    public bool IsBlocked { get; set; }

    /// <summary>
    /// User's email
    /// </summary>
    [MaxLength(64)]
    public string? Email { get; set; }

    /// <summary>
    /// User's roles
    /// </summary>
    public List<RoleToUser> Roles { get; set; } = [];
}