using Base.Permission.Core;

namespace Base.Permission.UseCases.Abstractions;

/// <summary>
/// Repository for accessing permissions
/// </summary>
public interface IPermissionRepository
{
    /// <summary>
    /// Get feature by name
    /// </summary>
    /// <param name="featureName"> Feature name </param>
    /// <returns> Feature model </returns>
    public Task<FeatureModel?> GetFeature(string featureName);
}