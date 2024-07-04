using Refit;

namespace Base.Permission.Clients;

/// <summary>
/// Client for checking access rights
/// </summary>
public interface IPermissionClient
{
    /// <summary>
    /// Check if the passed roles have access to the feature
    /// </summary>
    /// <param name="featureId"> Feature id </param>
    /// <param name="roles"> List of roles </param>
    /// <returns> <see langword="true"/> if access is allowed, otherwise <see langword="false"/>. </returns>
    [Get("/api/permission/features/{featureId}/isAvailable")]
    public Task<bool> HasAccess(string featureId, [Query(CollectionFormat.Multi)] string[] roles);
}