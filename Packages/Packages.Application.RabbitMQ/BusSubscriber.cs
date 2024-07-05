using Microsoft.Extensions.Hosting;

namespace Packages.Application.RabbitMQ;

/// <summary>
/// Service that runs in a background process to subscribe to queues
/// </summary>
/// <typeparam name="T"> Subscriber Type </typeparam>
internal class BusSubscriber<T> : IHostedService where T : AutoSubscriberWrapper
{
    private readonly T _subscriber;
    private IDisposable? _subscriptions;

    public BusSubscriber(T subscriber) => _subscriber = subscriber;

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _subscriptions = await _subscriber.SubscribeAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_subscriptions is IAsyncDisposable asyncDispose)
            await asyncDispose.DisposeAsync().ConfigureAwait(false);

        _subscriptions?.Dispose();
    }
}