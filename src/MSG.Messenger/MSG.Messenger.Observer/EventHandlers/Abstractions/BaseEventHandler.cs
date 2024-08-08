using MediatR;
using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications.Abstractions;

namespace MSG.Messenger.Observer.EventHandlers.Abstractions;

/// <summary>
/// Base handler for events
/// </summary>
/// <typeparam name="T"> Event to be handled </typeparam>
public abstract class BaseEventHandler<T> : INotificationHandler<T> where T : IBaseEvent
{
    protected readonly IHubContext<MessengerHub, IMessengerClient> MessengerHubContext;

    protected BaseEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    {
        MessengerHubContext = messengerHubContext;
    }

    /// <summary>
    /// Handles whether the event was successful or not
    /// </summary>
    /// <param name="notification"> The event </param>
    /// <param name="cancellationToken"> Cancellation token </param>
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        if (notification.IsSuccess)
        {
            await HandleEvent(notification, cancellationToken);
        }
        else
        {
            await ErrorNotification(notification.CallerConnectionId, notification.Errors);
        }
    }

    /// <summary>
    /// Handles event
    /// </summary>
    /// <param name="notification"> The event </param>
    /// <param name="cancellationToken"> Cancellation token </param>
    public abstract Task HandleEvent(T notification, CancellationToken cancellationToken);

    /// <summary>
    /// Returns a collection of errors to the client
    /// </summary>
    /// <param name="callerConnectionId"> Caller connection id </param>
    /// <param name="errors"> Collection of errors </param>
    private async Task ErrorNotification(string callerConnectionId, IReadOnlyCollection<string>? errors)
    {
        await MessengerHubContext.Clients
            .Client(callerConnectionId)
            .ReceivedErrors(errors!.ToArray());
    }
}