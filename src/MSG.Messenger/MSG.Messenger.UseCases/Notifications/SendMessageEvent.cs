using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class SendMessageEvent : IBaseEvent
{
    public MessageModel? Message { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public SendMessageEvent(Result<MessageModel> result, string callerConnectionId)
    {
        Message = result.GetValueOrDefault();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
    }
}