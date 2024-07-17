namespace MSG.Security.Authorization;

/// <summary>
/// Authorized client profile
/// </summary>
public interface IClientAccessor
{
    /// <summary>
    /// Client id
    /// </summary>
    public string Id { get; }
}