using MSG.Messenger.Core;

namespace MSG.Messenger.Observer.Hubs.Clients;

/// <summary>
/// Messenger client interface
/// </summary>
public interface IMessengerClient
{
    Task CreatedGroupChat(ChatModelResult chat);

    Task ReceivedNewMessage(MessageModel message);

    Task DeletedExistingMessage(MessageModel message);

    Task RedactedExistingMessage(MessageModel message);

    Task ReceivedErrors(string[] errors);
}