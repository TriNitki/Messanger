using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.UseCases.Notifications;

namespace MSG.Messenger.Observer.EventHandlers;

public class AddGroupChatMemberEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<AddGroupChatMemberEvent>(messengerHubContext)
{
    public override async Task HandleEvent(AddGroupChatMemberEvent notification, CancellationToken cancellationToken)
    {
        var groupName = notification.Chat!.Id.ToString();

        foreach (var addingMemberConnection in notification.AddingMemberConnections)
        {
            await MessengerHubContext.Groups.AddToGroupAsync(addingMemberConnection, groupName, cancellationToken);
        }

        await MessengerHubContext.Clients.Group(groupName)
            .NewGroupChatMember(notification.Chat!, (Guid)notification.AddingMemberId!);
    }
}