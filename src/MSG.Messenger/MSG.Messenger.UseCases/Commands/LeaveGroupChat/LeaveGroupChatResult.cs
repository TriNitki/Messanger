using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Commands.LeaveGroupChat;

public record LeaveGroupChatResult(ChatModelResult LeavingChat, Guid? NewAdminId = null);