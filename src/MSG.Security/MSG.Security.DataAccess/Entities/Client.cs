namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// Client
/// </summary>
public class Client
{
    /// <summary>
    /// ServiceName
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Hashed secret
    /// </summary>
    public string HashedSecret { get; set; }
}