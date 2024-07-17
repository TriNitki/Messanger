using System.Net.Http.Headers;
using MSG.Security.Authentication.Clients.Abstractions;

namespace MSG.Security.Authentication.Clients;

/// <summary>
/// Authentication header handler
/// </summary>
public class AuthHeaderHandler : DelegatingHandler
{
    /// <summary>
    /// Authentication schema
    /// </summary>
    private const string Schema = "Bearer";

    /// <summary>
    /// Authorization token provider for user
    /// </summary>
    private readonly IClientTokenProvider _tokenProvider;

    /// <summary>
    /// Authorization client
    /// </summary>
    private readonly IAuthorizationClient _authClient;

    public AuthHeaderHandler(IClientTokenProvider tokenProvider, IAuthorizationClient authClient)
    {
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _tokenProvider.GetAccessTokenAsync(_authClient);
        request.Headers.Authorization = new AuthenticationHeaderValue(Schema, accessToken);
        var result = await base.SendAsync(request, cancellationToken);

        if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            accessToken = await _tokenProvider.UpdateTokenAsync(_authClient);
            request.Headers.Authorization = new AuthenticationHeaderValue(Schema, accessToken);
            result = await base.SendAsync(request, cancellationToken);
        }

        return result;
    }
}
