using Base.Authentication.Clients.Abstractions;
using Base.Authentication.Contracts;
using Microsoft.Extensions.Options;

namespace Base.Authentication.Clients;

public class UserTokenProvider : IUserTokenProvider
{
    /// <summary>
    /// Access token
    /// </summary>
    private Token? _accessToken;

    /// <summary>
    /// Refresh token
    /// </summary>
    private Token? _refreshToken;

    /// <summary>
    /// Jet token options
    /// </summary>
    private readonly JwtTokenOptions _jwtOptions;

    /// <summary>
    /// Login request model
    /// </summary>
    private readonly LoginRequest _loginRequest;

    /// <summary>
    /// Semaphore
    /// </summary>
    private readonly SemaphoreSlim _semaphore;

    public UserTokenProvider(
        IOptions<JwtTokenOptions> jwtOptions, IOptions<UserOptions> userOptions)
    {
        var login = userOptions.Value.Login;
        var password = userOptions.Value.Password;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            throw new ArgumentException("Login or password for user is not specified");

        _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        _loginRequest = new LoginRequest(login, password);
        _semaphore = new SemaphoreSlim(1);
    }

    /// <inheritdoc/>
    public async Task<string> GetAccessTokenAsync(IAuthorizationClient client)
    {
        if (!_accessToken?.IsValid() ?? true)
        {
            return _refreshToken?.IsValid() ?? false
                ? await RefreshTokensAsync(client)
                : await LoginAsync(client);
        }

        return _accessToken!.Value;
    }

    /// <inheritdoc/>
    public async Task<string> UpdateTokensAsync(IAuthorizationClient client)
    {
        return await LoginAsync(client);
    }

    /// <summary>
    /// Refresh tokens
    /// </summary>
    /// <param name="client"> Authorization client </param>
    /// <returns> Access token </returns>
    private async Task<string> RefreshTokensAsync(IAuthorizationClient client)
    {
        await _semaphore.WaitAsync();

        try
        {
            if (!_accessToken?.IsValid() ?? true)
            {
                SetTokens(await client.RefreshTokens(new RefreshTokenRequest(_refreshToken!.Value)));
            }
        }
        catch
        {
            SetTokens(await client.Login(_loginRequest));
        }
        finally
        {
            _semaphore.Release();
        }

        return _accessToken!.Value;
    }

    /// <summary>
    /// Authorize
    /// </summary>
    /// <param name="client"> Authorization client </param>
    /// <returns> Access token </returns>
    private async Task<string> LoginAsync(IAuthorizationClient client)
    {
        await _semaphore.WaitAsync();

        try
        {
            if (!_accessToken?.IsValid() ?? true)
            {
                SetTokens(await client.RefreshTokens(
                    new RefreshTokenRequest(_refreshToken!.Value)));
            }
        }
        catch
        {
            SetTokens(await client.Login(_loginRequest));
        }
        finally
        {
            _semaphore.Release();
        }

        return _accessToken!.Value;
    }

    /// <summary>
    /// Set a pair of tokens
    /// </summary>
    /// <param name="tokens"> Pair of tokens </param>
    private void SetTokens(Tokens tokens)
    {
        _accessToken?.Invalidate();
        _refreshToken?.Invalidate();

        _accessToken = new Token(tokens.AccessToken,
            DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationInMinutes));

        _refreshToken = new Token(
            tokens.RefreshToken, tokens.RefreshTokenExpiration.UtcDateTime);
    }
}
