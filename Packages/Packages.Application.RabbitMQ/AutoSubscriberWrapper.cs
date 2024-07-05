using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;

namespace Packages.Application.RabbitMQ;

/// <summary>
/// Wrapper for <see cref="AutoSubscriber"/>.
/// </summary>
internal class AutoSubscriberWrapper
{
    private readonly IServiceResolver _serviceResolver;
    private readonly Type[] _consumers;
    private readonly string? _subscriptionIdPrefix;

    internal AutoSubscriberWrapper(IServiceResolver serviceResolver, Type[] consumers, string? subscriptionIdPrefix)
    {
        _serviceResolver = serviceResolver;
        _consumers = consumers;
        _subscriptionIdPrefix = subscriptionIdPrefix;
    }

    /// <summary>
    /// Subscribe to the queue
    /// </summary>
    /// <param name="cancellationToken"> Cancellation token </param>
    /// <returns> Subscription </returns>
    internal async Task<IDisposable> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        var autoSubscriber = new AutoSubscriber(_serviceResolver.Resolve<IBus>(), _subscriptionIdPrefix)
        {
            AutoSubscriberMessageDispatcher = new AsyncScopedAutoSubscriberMessageDispatcher(_serviceResolver),
            GenerateSubscriptionId = x => x.MessageType.Name.Split('.').Last()
        };

        return await autoSubscriber.SubscribeAsync(this._consumers, cancellationToken).ConfigureAwait(false);
    }
}