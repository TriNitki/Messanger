using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Commands.LeaveGroupChat;
using MSG.Messenger.UseCases.Notifications.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class LeaveGroupChatEvent : IBaseEvent
{
    public ChatModelResult? LeavingChat { get; set; }

    public Guid? LeavingMemberId { get; set; }

    public Guid? NewAdminId { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public HashSet<string>? LeaveMemberConnections { get; set; }

    public LeaveGroupChatEvent(Result<LeaveGroupChatResult> result, string callerConnectionId, Guid leavingMemberId, HashSet<string>? leaveMemberConnections)
    {
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
        LeaveMemberConnections = leaveMemberConnections;
        LeavingMemberId = leavingMemberId;

        if (IsSuccess)
        {
            var valueResult = result.GetValueOrDefault()!;
            LeavingChat = valueResult.LeavingChat;
            NewAdminId = valueResult.NewAdminId;
        }
    }
}