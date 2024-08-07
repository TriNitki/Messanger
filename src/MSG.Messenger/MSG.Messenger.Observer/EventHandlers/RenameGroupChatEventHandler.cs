using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.UseCases.Notifications;
using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.Observer.Hubs.Clients;

namespace MSG.Messenger.Observer.EventHandlers;

public class RenameGroupChatEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<RenameGroupChatEvent>(messengerHubContext)
{
    public override async Task HandleEvent(RenameGroupChatEvent notification, CancellationToken cancellationToken)
    {
        await MessengerHubContext.Clients.Group(notification.Chat!.Id.ToString())
            .RenamedGroupChat(notification.Chat);
    }
}