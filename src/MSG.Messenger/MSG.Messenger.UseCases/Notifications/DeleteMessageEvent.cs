using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class DeleteMessageEvent : IBaseEvent
{
    public MessageModel Message { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public DeleteMessageEvent(Result<MessageModel> result, string callerConnectionId)
    {
        Message = result.GetValue();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
    }
}