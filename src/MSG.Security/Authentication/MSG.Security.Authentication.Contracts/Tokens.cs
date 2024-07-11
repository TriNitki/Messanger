namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// Model for storing authorization tokens
/// </summary>
/// <param name="AccessToken"></param>
/// <param name="RefreshToken"></param>
/// <param name="RefreshTokenExpiration"></param>
public record Tokens(string AccessToken, string RefreshToken, DateTimeOffset RefreshTokenExpiration);