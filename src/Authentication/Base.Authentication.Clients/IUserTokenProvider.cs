namespace Base.Authentication.Clients;

/// <summary>
/// Authorizastion token provider for user
/// </summary>
public interface IUserTokenProvider
{
    /// <summary>
    /// Get access token
    /// </summary>
    /// <param name="client"> Authorization client </param>
    /// <returns> Access token </returns>
    public Task<string> GetAccessTokenAsync(IAuthorizationClient client);

    /// <summary>
    /// Update tokens
    /// </summary>
    /// <param name="client"> Authorization client </param>
    /// <returns> Access token </returns>
    public Task<string> UpdateTokensAsync(IAuthorizationClient client);
}
