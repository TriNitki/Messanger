using System.ComponentModel.DataAnnotations;

namespace MSG.Security.DataAccess.Entities;

/// <summary>
/// Client
/// </summary>
public class Client
{
    /// <summary>
    /// Client name
    /// </summary>
    [MaxLength(64)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Hashed secret
    /// </summary>
    [MinLength(64), MaxLength(64)]
    public string HashedSecret { get; set; } = string.Empty;
}