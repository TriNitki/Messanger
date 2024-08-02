using MSG.Messenger.UseCases.Notifications;
using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.EventHandlers.Abstractions;

namespace MSG.Messenger.Observer.EventHandlers;

public class SendMessageEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<SendMessageEvent>(messengerHubContext)
{
    public override async Task HandleEvent(SendMessageEvent notification, CancellationToken cancellationToken)
    {
        await MessengerHubContext.Clients
            .Group(notification.Message!.ChatId.ToString())
            .ReceivedNewMessage(notification.Message);
    }
}