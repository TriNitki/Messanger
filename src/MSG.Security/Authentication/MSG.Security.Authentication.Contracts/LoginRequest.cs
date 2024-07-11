namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// Login request model
/// </summary>
/// <param name="Login"> Login </param>
/// <param name="Password"> Password </param>
public record LoginRequest(string Login, string Password);