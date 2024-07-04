namespace Base.Authentication.Core;

/// <summary>
/// Refresh token
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Content
    /// </summary>
    public string Content { get; set;  }

    /// <summary>
    /// Expiration date time
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// Whether the token is used
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// User id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Token family id
    /// </summary>
    public Guid FamilyId { get; set; }

    /// <summary>
    /// Token family
    /// </summary>
    public RefreshTokenFamily? Family { get; set; }

    /// <summary>
    /// Check token validity
    /// </summary>
    /// <returns>
    /// <see langword="true"/>, if token valid, otherwise <see langword="false"/>
    /// </returns>
    public bool IsValid()
    {
        return !IsUsed && Expiration > DateTime.UtcNow;
    }

    /// <summary>
    /// Mark token as used
    /// </summary>
    public void Deactivate()
    {
        IsUsed = true;
    }
}
