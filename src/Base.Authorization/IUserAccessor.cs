namespace Base.Authorization;

/// <summary>
/// Authorized user profile
/// </summary>
public interface IUserAccessor
{
    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; }

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Roles
    /// </summary>
    public string[] Roles { get; }
}

