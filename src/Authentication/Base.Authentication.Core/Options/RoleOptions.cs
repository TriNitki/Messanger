namespace Base.Authentication.Core.Options;

/// <summary>
/// Role options
/// </summary>
public class RoleOptions
{
    /// <summary>
    /// List of default user roles
    /// </summary>
    public List<string> DefaultUserRoles { get; set; } = [];

    /// <summary>
    /// List of default service roles
    /// </summary>
    public List<string> DefaultServiceRoles { get; set; } = [];
}