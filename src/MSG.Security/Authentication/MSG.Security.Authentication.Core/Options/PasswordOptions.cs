namespace MSG.Security.Authentication.Core.Options;

/// <summary>
/// Password options
/// </summary>
public class PasswordOptions
{
    /// <summary>
    /// Password salt
    /// </summary>
    public string Salt { get; set; } = string.Empty;
}
