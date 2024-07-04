using Base.Authentication.Core;

namespace Base.Authentication.UseCases.Abstractions;

/// <summary>
/// Repository for accessing refresh token families
/// </summary>
public interface IRefreshTokenFamilyRepository
{
    /// <summary>
    /// Get the value associated with this id if it exists, or generate a new entity using the provided id
    /// </summary>
    /// <param name="id"> Token family id </param>
    /// <returns> Token family entity </returns>
    Task<RefreshTokenFamily> GetOrCreateByIdAsync(Guid id);

    /// <summary>
    /// Lock family by its id
    /// </summary>
    /// <param name="id"> Token family id </param>
    Task LockByIdAsync(RefreshTokenFamily id);
}