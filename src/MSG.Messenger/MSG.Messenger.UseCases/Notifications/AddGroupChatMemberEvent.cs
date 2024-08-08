using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Notifications.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class AddGroupChatMemberEvent : IBaseEvent
{
    public ChatModelResult? Chat { get; set; }

    public Guid? AddingMemberId { get; set; }

    public HashSet<string> AddingMemberConnections { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public AddGroupChatMemberEvent(Result<ChatModelResult> result, string callerConnectionId, Guid addingMemberId, HashSet<string> addingMemberConnections)
    {
        Chat = result.GetValueOrDefault();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
        AddingMemberId = addingMemberId;
        AddingMemberConnections = addingMemberConnections;
    }
}