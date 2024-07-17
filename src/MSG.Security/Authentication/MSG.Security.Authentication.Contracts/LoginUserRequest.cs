namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// User login request model
/// </summary>
/// <param name="Login"> Login </param>
/// <param name="Password"> Password </param>
public record LoginUserRequest(string Login, string Password);