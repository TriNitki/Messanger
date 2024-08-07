using MediatR;
using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications.Abstractions;

namespace MSG.Messenger.Observer.EventHandlers.Abstractions;

public abstract class BaseEventHandler<T> : INotificationHandler<T> where T : IBaseEvent
{
    protected readonly IHubContext<MessengerHub, IMessengerClient> MessengerHubContext;

    protected BaseEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    {
        MessengerHubContext = messengerHubContext;
    }

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

    public abstract Task HandleEvent(T notification, CancellationToken cancellationToken);

    private async Task ErrorNotification(string callerConnectionId, IReadOnlyCollection<string>? errors)
    {
        await MessengerHubContext.Clients
            .Client(callerConnectionId)
            .ReceivedErrors(errors!.ToArray());
    }
}