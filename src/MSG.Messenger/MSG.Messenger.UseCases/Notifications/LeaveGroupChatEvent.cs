using MSG.Messenger.UseCases.Commands.LeaveGroupChat;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class LeaveGroupChatEvent : IBaseEvent
{
    public HashSet<string> LeaveMemberConnections { get; set; }

    public HashSet<string>? NewAdminConnections { get; set; }

    public HashSet<string>? OtherMemberConnections { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public LeaveGroupChatEvent(
        Result<LeaveGroupChatResult> result, 
        HashSet<string> leaveMemberConnections, 
        HashSet<string>? newAdminConnections, 
        HashSet<string>? otherMemberConnections,
        string callerConnectionId
        )
    {
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
    }
}