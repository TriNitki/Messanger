using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;
using System.Reflection;

namespace Packages.Application.RabbitMQ;

/// <summary>
/// Auto message subscriber dispatcher with embedded async scope
/// </summary>
public class AsyncScopedAutoSubscriberMessageDispatcher : IAutoSubscriberMessageDispatcher
{
    private readonly IServiceResolver _resolver;

    private static readonly Func<object?, object?> InnerScope =
        Type.GetType("EasyNetQ.DI.Microsoft.ServiceCollectionAdapter+MicrosoftServiceResolverScope,EasyNetQ.DI.Microsoft")!
            .GetField("serviceScope", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue;

    public AsyncScopedAutoSubscriberMessageDispatcher(IServiceResolver resolver)
    {
        _resolver = resolver;
    }

    /// <inheritdoc />
    public void Dispatch<TMessage, TConsumer>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class
        where TConsumer : class, IConsume<TMessage>
    {
        using var scope = _resolver.CreateScope();
        var consumer = scope.Resolve<TConsumer>();
        consumer.Consume(message, cancellationToken);
    }

    /// <inheritdoc />
    public async Task DispatchAsync<TMessage, TAsyncConsumer>(TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class
        where TAsyncConsumer : class, IConsumeAsync<TMessage>
    {
        using var scope = _resolver.CreateScope();
        try
        {
            var asyncConsumer = scope.Resolve<TAsyncConsumer>();
            await asyncConsumer.ConsumeAsync(message, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (scope is IAsyncDisposable ad)
            {
                await ad.DisposeAsync();
            }
            else if (InnerScope(scope) is IAsyncDisposable innerScope)
            {
                await innerScope.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}