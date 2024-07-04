namespace Base.Authentication.Contracts;

/// <summary>
/// Change password reqest model 
/// </summary>
/// <param name="Login"> Login </param>
/// <param name="Password"> Password </param>
/// <param name="newPassword"> New password </param>
public record ChangePasswordRequest(string Login, string Password, string newPassword);