using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// Feature
/// </summary>
public class Feature
{
    /// <summary>
    /// Name
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description
    /// </summary>
    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of roles, that have access to the feature
    /// </summary>
    public List<RoleToFeature> Roles { get; set; } = [];
}