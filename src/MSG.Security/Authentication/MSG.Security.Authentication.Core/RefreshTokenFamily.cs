namespace MSG.Security.Authentication.Core;

/// <summary>
/// Refresh token family
/// </summary>
public class RefreshTokenFamily
{
    /// <summary>
    /// Token family id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Whether the token family is locked 
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Mark token family as locked
    /// </summary>
    public void Lock()
    {
        IsLocked = true;
    }
}