namespace MSG.Messenger.Contracts;

public record CreateGroupChatRequest(string Name, HashSet<Guid> Members);