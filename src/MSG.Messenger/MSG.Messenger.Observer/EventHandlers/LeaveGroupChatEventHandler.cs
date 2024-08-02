using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;

namespace MSG.Messenger.Observer.EventHandlers;

public class LeaveGroupChatEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<LeaveGroupChatEvent>(messengerHubContext)
{
    public override Task HandleEvent(LeaveGroupChatEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}