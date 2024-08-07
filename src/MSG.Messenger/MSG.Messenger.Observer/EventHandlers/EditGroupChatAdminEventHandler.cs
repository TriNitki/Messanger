using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.EventHandlers.Abstractions;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.Observer.Hubs;
using MSG.Messenger.UseCases.Notifications;

namespace MSG.Messenger.Observer.EventHandlers;

public class EditGroupChatAdminEventHandler(IHubContext<MessengerHub, IMessengerClient> messengerHubContext)
    : BaseEventHandler<EditGroupChatAdminEvent>(messengerHubContext)
{
    public override async Task HandleEvent(EditGroupChatAdminEvent notification, CancellationToken cancellationToken)
    {
        var groupName = notification.Chat!.Id.ToString();

        if ((bool)notification.IsAdmin!)
        {
            await MessengerHubContext.Clients.Group(groupName)
                .MemberAdminPromotion(notification.Chat, (Guid)notification.MemberId!);
        }
        else
        {
            await MessengerHubContext.Clients.Group(groupName)
                .MemberAdminDemotion(notification.Chat, (Guid)notification.MemberId!);
        }
    }
}