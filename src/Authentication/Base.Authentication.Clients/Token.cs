namespace Base.Authentication.Clients;

/// <summary>
/// Authorization token
/// </summary>
internal class Token
{
    /// <summary>
    /// Expiration date
    /// </summary>
    private readonly DateTime _expirationDate;

    /// <summary>
    /// Whether token is valid 
    /// </summary>
    private bool _isValid;

    /// <summary>
    /// Token value
    /// </summary>
    internal string Value { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value"> Token value </param>
    /// <param name="expirationDate"> Expiration date </param>
    internal Token(string value, DateTime expirationDate)
    {
        Value = value;
        _expirationDate = expirationDate;
        _isValid = true;
    }

    /// <summary>
    /// Check token validity
    /// </summary>
    /// <returns> <see langword="true"/>, if token is valid, otherwise <see langword="false"/> </returns>
    internal bool IsValid()
    {
        return _isValid && _expirationDate > DateTime.UtcNow;
    }

    /// <summary>
    /// Invalidate token
    /// </summary>
    internal void Invalidate() => _isValid = false;
}
