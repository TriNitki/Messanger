namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// Refresh token request model
/// </summary>
/// <param name="RefreshToken"> Refresh token </param>
public record RefreshTokenRequest(string RefreshToken);
