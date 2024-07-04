namespace Base.DataAccess.Entities;

/// <summary>
/// User
/// </summary>
public class User
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Whether user is blocked
    /// </summary>
    public bool IsBlocked { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Roles
    /// </summary>
    public List<RoleToUser> Roles { get; set; } = [];
}