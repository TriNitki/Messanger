namespace Base.DataAccess.Entities;

/// <summary>
/// Role to feature entity
/// </summary>
public class RoleToFeature
{
    /// <summary>
    /// Role id (name)
    /// </summary>
    public string RoleId { get; set; } = string.Empty;

    /// <summary>
    /// Feature id
    /// </summary>
    public string FeatureId { get; set; } = string.Empty;

    /// <summary>
    /// Feature
    /// </summary>
    public Feature Feature { get; set; } = null!;
}