using MSG.Messenger.Core;

namespace MSG.Messenger.Observer.Hubs.Clients;

/// <summary>
/// Messenger client interface
/// </summary>
public interface IMessengerClient
{
    Task CreatedChat(ChatModelResult chat);

    Task MemberLeftGroupChat(ChatModelResult chat, Guid memberId);

    Task ChatDeleted(Guid chatId);

    Task MemberKickedFromGroupChat(ChatModelResult chat, Guid adminId, Guid kickedMemberId);

    Task YouAddedToGroupChat(ChatModelResult chat);

    Task NewGroupChatMember(ChatModelResult chat, Guid memberId);

    Task MemberAdminPromotion(ChatModelResult chat, Guid memberId);

    Task MemberAdminDemotion(ChatModelResult chat, Guid memberId);

    Task RenamedGroupChat(ChatModelResult chat);

    Task ReceivedNewMessage(ChatMessageModel chatMessage);

    Task DeletedExistingMessage(ChatMessageModel chatMessage);

    Task RedactedExistingMessage(ChatMessageModel chatMessage);

    Task ReceivedErrors(string[] errors);
}