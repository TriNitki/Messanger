namespace MSG.Security.Permission.Core;

/// <summary>
/// Feature
/// </summary>
public class FeatureModel
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Whitelist containing the user roles that have access to this feature.
    /// </summary>
    public HashSet<string> WhiteList { get; set; } = [];

    /// <summary>
    /// Check the availability of the feature to the passed roles
    /// </summary>
    /// <param name="roles"> Features </param>
    /// <returns> <see langword="true"/> if access is available, otherwise <see langword="false"/> </returns>
    public bool IsAvailable(IReadOnlyCollection<string> roles)
    {
        return WhiteList.Any(x => roles.Where(role => !string.IsNullOrWhiteSpace(role))
            .Any(role => role.Equals(x, StringComparison.OrdinalIgnoreCase)));
    }
}