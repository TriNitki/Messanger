using System.Security.Claims;

namespace MSG.Security.Authentication.Core.Abstractions;

/// <summary>
/// Authenticated customer interface
/// </summary>
public interface IAuthenticatedCustomer
{
    /// <summary>
    /// Get customer claims
    /// </summary>
    /// <returns> List of claims </returns>
    public Claim[] GetClaims();
}