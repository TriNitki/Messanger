using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;

namespace MSG.Messenger.Observer.EventHandlers;

public class CreateChatEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<CreateChatEvent>(messengerHubContext)
{
    public override async Task HandleEvent(CreateChatEvent notification, CancellationToken cancellationToken)
    {
        foreach (var receiverConnectionId in notification.ReceiverConnectionIds)
        {
            await MessengerHubContext.Groups.AddToGroupAsync(receiverConnectionId,
                notification.Chat.Id.ToString(), cancellationToken);
        }

        await MessengerHubContext.Clients.Group(notification.Chat.Id.ToString())
            .CreatedGroupChat(notification.Chat);
    }
}