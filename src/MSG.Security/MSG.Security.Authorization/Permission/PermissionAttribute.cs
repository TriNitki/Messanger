using Microsoft.AspNetCore.Authorization;

namespace MSG.Security.Authorization.Permission;

/// <summary>
/// Permission based authorization attribute
/// </summary>
public class PermissionAttribute : AuthorizeAttribute
{
    public const string PolicyPrefix = "Permission.";

    public PermissionAttribute(string feature)
    {
        Policy = PolicyPrefix + feature;
    }
}