﻿using MSG.Messenger.Core;

namespace MSG.Messenger.Observer.Hubs.Clients;

/// <summary>
/// Messenger client interface
/// </summary>
public interface IMessengerClient
{
    Task CreatedChat(ChatModelResult chat);

    Task MemberLeftGroupChat(ChatModelResult chat, Guid memberId);

    Task MemberKickedFromGroupChat(ChatModelResult chat, Guid adminId, Guid kickedMemberId);

    Task NewGroupChatMember(ChatModelResult chat, Guid memberId);

    Task MemberAdminPromotion(ChatModelResult chat, Guid memberId);

    Task MemberAdminDemotion(ChatModelResult chat, Guid memberId);

    Task RenamedGroupChat(ChatModelResult chat);

    Task ReceivedNewMessage(MessageModel message);

    Task DeletedExistingMessage(MessageModel message);

    Task RedactedExistingMessage(MessageModel message);

    Task ReceivedErrors(string[] errors);
}