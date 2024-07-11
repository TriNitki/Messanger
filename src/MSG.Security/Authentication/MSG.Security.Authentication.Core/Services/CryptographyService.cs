using System.Security.Cryptography;
using System.Text;

namespace MSG.Security.Authentication.Core.Services;

/// <summary>
/// Cryptography service
/// </summary>
public static class CryptographyService
{
    /// <summary>
    /// Get password hash
    /// </summary>
    /// <param name="password"> Password </param>
    /// <param name="salt"> Password salt </param>
    /// <returns> Hashed password </returns>
    public static string HashPassword(string password, string salt)
    {
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        byte[] byteString = Encoding.UTF8.GetBytes(password + saltBytes);
        var hash = SHA256.HashData(byteString);

        return BitConverter.ToString(hash).Replace("-", "");
    }
}
