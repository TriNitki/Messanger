using Microsoft.AspNetCore.Authorization;

namespace MSG.Security.Authorization.Permission;

/// <summary>
/// Permission based authorization attribute
/// </summary>
public class PermissionAttribute : AuthorizeAttribute
{
    public const string POLICY_PREFIX = "Permission.";

    public PermissionAttribute(string feature)
    {
        Policy = POLICY_PREFIX + feature;
    }
}