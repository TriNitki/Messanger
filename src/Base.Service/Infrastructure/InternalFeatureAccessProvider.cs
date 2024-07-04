using Base.Permission.Clients;
using Base.Permission.UseCases;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Base.Service.Infrastructure;

/// <summary>
/// <see cref="IFeatureAccessProvider"/>  implementation for internal authorisation
/// </summary>
public class InternalFeatureAccessProvider : IFeatureAccessProvider
{
    private readonly IMemoryCache _cache;
    private readonly IMediator _mediator;

    public InternalFeatureAccessProvider(IMemoryCache cache, IMediator mediator)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <inheritdoc/>
    public async Task<bool> HasAccess(string feature, string[] userRoles)
    {
        var key = new PermissionCacheKey(feature, userRoles);

        return await _cache.GetOrCreateAsync(key, async x =>
        {
            x.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            var result = await _mediator.Send(new CheckFeatureAccessQuery(feature, userRoles));
            return result.IsSuccess && result.GetValue();
        });
    }
}