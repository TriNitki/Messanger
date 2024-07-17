using MSG.Security.Authentication.Core;

namespace MSG.Security.Authentication.UseCases.Abstractions;

/// <summary>
/// Repository for accessing clients
/// </summary>
public interface IClientRepository
{
    /// <summary>
    /// Get client
    /// </summary>
    /// <param name="name"> Client name </param>
    /// <returns> Client </returns>
    Task<AuthClient?> ResolveAsync(string name);

    /// <summary>
    /// Authenticate client
    /// </summary>
    /// <param name="name"> Client name </param>
    /// <param name="secret"> Client secret </param>
    /// <returns> Authenticated client </returns>
    Task<AuthClient?> ResolveAsync(string name, string secret);

    /// <summary>
    /// Create client
    /// </summary>
    /// <param name="client"> Client </param>
    /// <returns> Created client </returns>
    Task<AuthClient> CreateAsync(AuthClient client);
}