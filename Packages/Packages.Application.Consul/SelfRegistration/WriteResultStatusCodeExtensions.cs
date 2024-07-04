using Consul;

namespace Packages.Application.Consul.SelfRegistration;

internal static class WriteResultStatusCodeExtensions
{
    public static bool IsOk(this WriteResult? result)
    {
        if (result == null)
            return false;
        var statusCode = (int)result.StatusCode;
        return statusCode is >= 200 and < 300;
    }
}