using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;
using MSG.Messenger.Observer.EventHandlers.Abstractions;

namespace MSG.Messenger.Observer.EventHandlers;

public class DeleteMessageEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<DeleteMessageEvent>(messengerHubContext)
{
    public override async Task HandleEvent(DeleteMessageEvent notification, CancellationToken cancellationToken)
    {
        await MessengerHubContext.Clients
            .Group(notification.Message!.ChatId.ToString())
            .DeletedExistingMessage(notification.Message);
    }
}