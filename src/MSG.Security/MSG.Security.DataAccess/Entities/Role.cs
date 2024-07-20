using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// Role
/// </summary>
public class Role
{
    /// <summary>
    /// Role name
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Role description
    /// </summary>
    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty;
}