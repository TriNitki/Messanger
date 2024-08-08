using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;

namespace MSG.Messenger.Observer.EventHandlers;

public class LeaveGroupChatEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<LeaveGroupChatEvent>(messengerHubContext)
{
    public override async Task HandleEvent(LeaveGroupChatEvent notification, CancellationToken cancellationToken)
    {
        var groupName = notification.LeavingChat!.Id.ToString();

        foreach (var leavingConnections in notification.LeaveMemberConnections!)
        {
            await MessengerHubContext.Groups.RemoveFromGroupAsync(
                leavingConnections, groupName, cancellationToken);
        }

        await MessengerHubContext.Clients.User(notification.LeavingMemberId.ToString()!)
            .ChatDeleted(notification.LeavingChat!.Id);
        await MessengerHubContext.Clients.Group(groupName)
            .MemberLeftGroupChat(notification.LeavingChat!, (Guid)notification.LeavingMemberId!);

        if (notification.NewAdminId != null)
        {
            await MessengerHubContext.Clients.Group(groupName)
                .MemberAdminPromotion(notification.LeavingChat!, (Guid)notification.NewAdminId);
        }
    }
}