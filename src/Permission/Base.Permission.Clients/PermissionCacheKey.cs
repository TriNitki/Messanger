namespace Base.Permission.Clients;

/// <summary>
/// Key for caching permissions
/// </summary>
public class PermissionCacheKey : IEquatable<PermissionCacheKey>
{
    /// <summary>
    /// Feature
    /// </summary>
    public string Feature { get; }
    
    /// <summary>
    /// Array of roles
    /// </summary>
    public string[] Roles { get; }

    public PermissionCacheKey(string feature, string[] roles)
    {
        Feature = feature;
        Roles = roles;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is PermissionCacheKey other)
        {
            return Equals(other);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(PermissionCacheKey? other)
    {
        if (other is null)
            return false;

        return other.Feature == Feature && EqualsRoles(other.Roles, Roles);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashRoles = 0;

        foreach (var role in Roles)
            hashRoles = HashCode.Combine(hashRoles, role.GetHashCode());

        return HashCode.Combine(Feature, hashRoles);
    }

    /// <summary>
    /// Check if the role arrays are the same
    /// </summary>
    /// <param name="firstRoles"> First array of roles </param>
    /// <param name="secondRoles"> Second array of roles </param>
    /// <returns> <see langword="true"/> if they're the same, otherwise <see langword="false"/> </returns>
    private static bool EqualsRoles(string[] firstRoles, string[] secondRoles)
    {
        return ReferenceEquals(firstRoles, secondRoles) || firstRoles.SequenceEqual(secondRoles);
    }
}