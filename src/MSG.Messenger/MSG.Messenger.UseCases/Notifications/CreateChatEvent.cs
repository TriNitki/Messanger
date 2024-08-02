using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class CreateChatEvent : IBaseEvent
{
    public ChatModelResult Chat { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public HashSet<string> ReceiverConnectionIds { get; set; }

    public CreateChatEvent(Result<ChatModelResult> result, string callerConnectionId, HashSet<string> receiverConnectionIds)
    {
        Chat = result.GetValue();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
        ReceiverConnectionIds = receiverConnectionIds;
    }
}
