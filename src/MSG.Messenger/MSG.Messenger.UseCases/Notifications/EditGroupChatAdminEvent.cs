using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Notifications.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class EditGroupChatAdminEvent : IBaseEvent
{
    public ChatModelResult? Chat { get; set; }

    public bool? IsAdmin { get; set; }

    public Guid? MemberId { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public EditGroupChatAdminEvent(Result<ChatModelResult> result, string callerConnectionId, bool? isAdmin, Guid? memberId)
    {
        Chat = result.GetValueOrDefault();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
        IsAdmin = isAdmin;
        MemberId = memberId;
    }
}