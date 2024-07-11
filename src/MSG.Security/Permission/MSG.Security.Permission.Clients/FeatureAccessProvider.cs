using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace MSG.Security.Permission.Clients;

/// <summary>
/// <see cref="IFeatureAccessProvider"/> implementation
/// </summary>
public class FeatureAccessProvider : IFeatureAccessProvider
{
    private readonly IPermissionClient _permissionClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<FeatureAccessProvider> _logger;

    public FeatureAccessProvider(
        IPermissionClient permissionClient,
        IMemoryCache cache,
        ILogger<FeatureAccessProvider> logger)
    {
        _permissionClient = permissionClient ?? throw new ArgumentNullException(nameof(permissionClient));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<bool> HasAccess(string feature, string[] userRoles)
    {
        var key = new PermissionCacheKey(feature, userRoles);

        return await _cache.GetOrCreateAsync(key, async x =>
        {
            x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            try
            {
                return await _permissionClient.HasAccess(feature, userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError("Permission check ended with an error - {message}", ex.Message);
                return false;
            }
        });
    }
}