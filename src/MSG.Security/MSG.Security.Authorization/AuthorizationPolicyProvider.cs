using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MSG.Security.Authorization.Client;
using MSG.Security.Authorization.Permission;

namespace MSG.Security.Authorization;

/// <summary>
/// Authorization policy provider
/// </summary>
public class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    /// <summary>
    /// All policies
    /// </summary>
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _allPolicies;

    /// <summary>
    /// Default authorization policy provider
    /// </summary>
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider;

    public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _allPolicies = new ConcurrentDictionary<string, AuthorizationPolicy>();
        _defaultPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    /// <summary>
    /// Get default policy
    /// </summary>
    /// <returns> Default policy </returns>
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _defaultPolicyProvider.GetDefaultPolicyAsync();
    }

    /// <summary>
    /// Get fallback policy
    /// </summary>
    /// <returns> Fallback policy </returns>
    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _defaultPolicyProvider.GetDefaultPolicyAsync()!;
    }

    /// <summary>
    /// Get authorization policy
    /// </summary>
    /// <param name="policyName"> Policy name </param>
    /// <returns> Authorization policy  </returns>
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PermissionAttribute.PolicyPrefix))
        {
            return _allPolicies.GetOrAdd(policyName, CreatePermissionPolicy);
        }
        
        if (policyName.StartsWith(ClientAttribute.DefaultPolicyName))
        {
            return _allPolicies.GetOrAdd(policyName, CreateClientPolicy);
        }

        return await _defaultPolicyProvider.GetPolicyAsync(policyName);
    }

    private static AuthorizationPolicy CreatePermissionPolicy(string policyName)
    {
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName[PermissionAttribute.PolicyPrefix.Length..]))
            .Build();
    }

    private static AuthorizationPolicy CreateClientPolicy(string policyName)
    {
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new ClientRequirement())
            .Build();
    }
}
