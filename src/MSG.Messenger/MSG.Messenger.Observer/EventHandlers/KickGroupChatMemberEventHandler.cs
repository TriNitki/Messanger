using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;

namespace MSG.Messenger.Observer.EventHandlers;

public class KickGroupChatMemberEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<KickGroupChatMemberEvent>(messengerHubContext)
{
    public override async Task HandleEvent(KickGroupChatMemberEvent notification, CancellationToken cancellationToken)
    {
        var groupName = notification.KickingChat!.Id.ToString();

        await MessengerHubContext.Clients.Group(groupName).MemberKickedFromGroupChat(
            notification.KickingChat!, (Guid)notification.AdminId!, (Guid)notification.KickingMemberId!);

        foreach (var kickingConnections in notification.KickingMemberConnections!)
        {
            await MessengerHubContext.Groups.RemoveFromGroupAsync(
                kickingConnections, groupName, cancellationToken);
        }
    }
}