using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

public interface IChatRepository
{
    public Task<ChatModel> CreateAsync(ChatModel chat);

    public Task UpdateAsync(ChatModel chat);
    
    public Task<ChatModel> GetOrCreateDirectAsync(Guid senderId, Guid receiverId);

    public Task<ChatModel?> GetByIdAsync(Guid chatId, bool includeMembers = false, bool ? isDirect = null);

    public Task<List<ChatModel?>> GetByUserIdAsync(Guid memberId, int? fromChat = null, int? toChat = null);

    public Task MarkAsDeletedAsync(ChatModel chat, bool deleteMembers = false);
}