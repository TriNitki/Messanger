using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;
using MSG.Messenger.Observer.EventHandlers.Abstractions;

namespace MSG.Messenger.Observer.EventHandlers;

public class RedactMessageEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<RedactMessageEvent>(messengerHubContext)
{
    public override async Task HandleEvent(RedactMessageEvent notification, CancellationToken cancellationToken)
    {
        await MessengerHubContext.Clients
            .Group(notification.Message!.ChatId.ToString())
            .RedactedExistingMessage(notification.Message);
    }
}