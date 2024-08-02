namespace MSG.Messenger.UseCases.Commands.LeaveGroupChat;

public record LeaveGroupChatResult(Guid LeaveMemberId, List<Guid>? OtherMemberIds = null, Guid? NewAdminId = null);