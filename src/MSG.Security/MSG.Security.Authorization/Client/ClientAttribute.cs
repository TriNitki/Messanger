using Microsoft.AspNetCore.Authorization;

namespace MSG.Security.Authorization.Client;

/// <summary>
/// Client credential flow authorization attribute
/// </summary>
public class ClientAttribute : AuthorizeAttribute
{
    public const string DefaultPolicyName = "ClientAttributePolicy";

    public ClientAttribute()
    {
        Policy = DefaultPolicyName;
    }
}