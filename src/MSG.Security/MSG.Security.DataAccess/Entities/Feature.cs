// ReSharper disable All

using MSG.Security.DataAccess.Entities;

namespace Base.DataAccess.Entities;

/// <summary>
/// Feature
/// </summary>
public class Feature
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of roles, that have access to the feature
    /// </summary>
    public List<RoleToFeature> Roles { get; set; } = [];
}