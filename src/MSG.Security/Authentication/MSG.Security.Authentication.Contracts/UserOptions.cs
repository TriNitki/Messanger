namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// User options
/// </summary>
public class UserOptions
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Roles
    /// </summary>
    public string[] Roles { get; set; } = [];
}
