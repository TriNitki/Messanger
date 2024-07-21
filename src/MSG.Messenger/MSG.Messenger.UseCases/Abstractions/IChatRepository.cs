using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

public interface IChatRepository
{
    public Task<ChatModel?> CreateAsync(ChatModel chat);
    
    public Task<ChatModel> GetOrCreateDirectAsync(Guid senderId, Guid receiverId);

    public Task<bool> LeaveGroupAsync(Guid userId, Guid chatId);
}