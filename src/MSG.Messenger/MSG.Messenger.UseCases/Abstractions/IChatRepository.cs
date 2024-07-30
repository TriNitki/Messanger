using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

/// <summary>
/// Chat repository interface
/// </summary>
public interface IChatRepository
{
    /// <summary>
    /// Create new chat
    /// </summary>
    /// <param name="chat"> Chat model </param>
    /// <returns> Created chat </returns>
    public Task<ChatModel> CreateAsync(ChatModel chat);

    /// <summary>
    /// Update existing chat
    /// </summary>
    /// <param name="chat"> Updated chat model </param>
    public Task UpdateAsync(ChatModel chat);
    
    /// <summary>
    /// Get or create direct chat
    /// </summary>
    /// <param name="senderId"> Sender member id </param>
    /// <param name="receiverId"> Receiver member id </param>
    /// <returns> Chat model </returns>
    public Task<ChatModel> GetOrCreateDirectAsync(Guid senderId, Guid receiverId);

    /// <summary>
    /// Get chat by id
    /// </summary>
    /// <param name="chatId"> Chat id </param>
    /// <param name="includeMembers"> Whether members should be included </param>
    /// <param name="isDirect"> Whether chat need to be specific type (<see langword="true"/> for group and <see langword="false"/> for direct) </param>
    /// <returns> Chat model </returns>
    public Task<ChatModel?> GetByIdAsync(Guid chatId, bool includeMembers = false, bool? isDirect = null);

    /// <summary>
    /// Get chats by user id
    /// </summary>
    /// <param name="memberId"> Member id </param>
    /// <param name="fromChat"> First chat index </param>
    /// <param name="toChat"> Last chat index </param>
    /// <returns> List of chat models </returns>
    public Task<List<ChatModel>> GetByUserIdAsync(Guid memberId, int? fromChat = null, int? toChat = null);

    /// <summary>
    /// Mark chat as deleted
    /// </summary>
    /// <param name="chat"> Chat model </param>
    /// <param name="deleteMembers"> Whether chat members need to be deleted </param>
    public Task MarkAsDeletedAsync(ChatModel chat, bool deleteMembers = false);
}