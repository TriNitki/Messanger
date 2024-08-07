using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Notifications.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Notifications;

public class KickGroupChatMemberEvent : IBaseEvent
{
    public ChatModelResult? KickingChat { get; set; }

    public HashSet<string>? KickingMemberConnections { get; set; }

    public Guid? AdminId { get; set; }

    public Guid? KickingMemberId { get; set; }

    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }

    public KickGroupChatMemberEvent(Result<ChatModelResult> result, Guid adminId, Guid kickingMemberId, HashSet<string>? kickingMemberConnections, string callerConnectionId)
    {
        KickingChat = result.GetValueOrDefault();
        IsSuccess = result.IsSuccess;
        Errors = result.Errors;
        CallerConnectionId = callerConnectionId;
        AdminId = adminId;
        KickingMemberId = kickingMemberId;
        KickingMemberConnections = kickingMemberConnections;
    }
}