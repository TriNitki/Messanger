using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Notifications.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class DeleteMessageEvent : IBaseEvent
{
    public ChatMessageModel? Message { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public DeleteMessageEvent(Result<ChatMessageModel> result, string callerConnectionId)
    {
        Message = result.GetValueOrDefault();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
    }
}