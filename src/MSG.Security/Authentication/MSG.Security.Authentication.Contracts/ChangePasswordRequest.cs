namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// Change password request model 
/// </summary>
/// <param name="Login"> Login </param>
/// <param name="Password"> Password </param>
/// <param name="NewPassword"> New password </param>
public record ChangePasswordRequest(string Login, string Password, string NewPassword);