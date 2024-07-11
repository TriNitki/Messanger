using Microsoft.AspNetCore.Authorization;

namespace MSG.Security.Authorization.Permission;

/// <summary>
/// Requirement for access to the feature
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Feature
    /// </summary>
    public string Feature { get; }

    public PermissionRequirement(string feature)
    {
        Feature = feature;
    }
}