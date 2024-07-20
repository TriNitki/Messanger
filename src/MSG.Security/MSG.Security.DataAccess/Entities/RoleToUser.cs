using System.ComponentModel.DataAnnotations;

namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// Role to user entity
/// </summary>
public class RoleToUser
{
    /// <summary>
    /// Role id (name)
    /// </summary>
    [MaxLength(64)]
    public string RoleId { get; set; } = string.Empty;

    /// <summary>
    /// User id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User
    /// </summary>
    public User User { get; set; } = null!;
}