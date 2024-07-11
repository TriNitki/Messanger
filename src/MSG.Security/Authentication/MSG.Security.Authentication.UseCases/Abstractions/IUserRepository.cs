using MSG.Security.Authentication.Core;

namespace MSG.Security.Authentication.UseCases.Abstractions;

/// <summary>
/// Repository for accessing users
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Get by user id
    /// </summary>
    /// <param name="id"> User id </param>
    /// <returns> User </returns>
    Task<AuthUser?> GetByIdAsync(Guid id);

    /// <summary>
    /// Get user by login
    /// </summary>
    /// <param name="login"> Login </param>
    /// <returns> User </returns>
    Task<AuthUser?> ResolveAsync(string login);

    /// <summary>
    /// Get user by login and password
    /// </summary>
    /// <param name="login"> Login </param>
    /// <param name="password"> Password </param>
    /// <returns> User </returns>
    Task<AuthUser?> ResolveAsync(string login, string password);

    /// <summary>
    /// Create user
    /// </summary>
    /// <param name="user"> User </param>
    /// <returns> Created user </returns>
    Task<AuthUser> CreateAsync(AuthUser user);

    /// <summary>
    /// Update existing user
    /// </summary>
    /// <param name="user"> Updated user </param>
    Task UpdateAsync(AuthUser user);
}
