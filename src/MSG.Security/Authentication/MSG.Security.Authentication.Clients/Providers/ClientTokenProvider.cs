using Microsoft.Extensions.Options;
using MSG.Security.Authentication.Clients.Abstractions;
using MSG.Security.Authentication.Contracts;

namespace MSG.Security.Authentication.Clients.Providers;

public class ClientTokenProvider : IClientTokenProvider
{
    /// <summary>
    /// Access token
    /// </summary>
    private Token? _accessToken;

    /// <summary>
    /// Jet token options
    /// </summary>
    private readonly JwtTokenOptions _jwtOptions;

    /// <summary>
    /// Login client clientRequest model
    /// </summary>
    private readonly LoginClientRequest _loginClientRequest;

    public ClientTokenProvider(
        IOptions<JwtTokenOptions> jwtOptions, IOptions<ClientOptions> userOptions)
    {
        var name = userOptions.Value.ServiceName;
        var secret = userOptions.Value.ServiceSecret;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(secret))
            throw new ArgumentException("Service name or secret for client is not specified");

        _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        _loginClientRequest = new LoginClientRequest(name, secret);
    }

    /// <inheritdoc/>
    public async Task<string> GetAccessTokenAsync(IAuthorizationClient client)
    {
        if (!_accessToken?.IsValid() ?? true)
        {
            return await LoginAsync(client);
        }

        return _accessToken!.Value;
    }

    /// <inheritdoc/>
    public async Task<string> UpdateTokenAsync(IAuthorizationClient client)
    {
        return await LoginAsync(client);
    }

    /// <summary>
    /// Authorize
    /// </summary>
    /// <param name="client"> Authorization client </param>
    /// <returns> Access token </returns>
    private async Task<string> LoginAsync(IAuthorizationClient client)
    {
        var token = (await client.Login(_loginClientRequest)).Content!;

        _accessToken?.Invalidate();
        _accessToken = new Token(token,
            DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationInMinutes));

        return _accessToken!.Value;
    }
}
