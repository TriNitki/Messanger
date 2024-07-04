namespace Base.Permission.Clients;

/// <summary>
/// Feature access provider
/// </summary>
public interface IFeatureAccessProvider
{
    /// <summary>
    /// Check if the passed roles have access to the feature
    /// </summary>
    /// <param name="feature"> Feature </param>
    /// <param name="userRoles"> List of user roles </param>
    /// <returns> <see langword="true"/>, if access is allowed, otherwise <see langword="false"/> </returns>
    public Task<bool> HasAccess(string feature, string[] userRoles);
}