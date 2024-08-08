using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

/// <summary>
/// Message repository interface
/// </summary>
public interface IChatMessageRepository
{
    /// <summary>
    /// Get messages by chat id
    /// </summary>
    /// <param name="chatId"> Chat id </param>
    /// <param name="fromMessage"> First message index </param>
    /// <param name="toMessage"> Last message index </param>
    /// <param name="sortByNewest"> Whether messages should be sorted by newest </param>
    /// <param name="excludeDeleted"> Whether deleted messages should be ignored </param>
    /// <returns> List of messages </returns>
    public Task<List<ChatMessageModel>> GetByChatIdAsync(
        Guid chatId, int? fromMessage = null, int? toMessage = null, bool sortByNewest = true, bool excludeDeleted = true);

    /// <summary>
    /// Get message by id
    /// </summary>
    /// <param name="messageId"> Message id </param>
    /// <returns> Message model </returns>
    public Task<ChatMessageModel?> GetByIdAsync(Guid messageId);

    /// <summary>
    /// Create chatMessage
    /// </summary>
    /// <param name="chatMessage"> Message model </param>
    public Task CreateAsync(ChatMessageModel chatMessage);
    
    /// <summary>
    /// Update existing chatMessage
    /// </summary>
    /// <param name="chatMessage"> Message model </param>
    public Task UpdateAsync(ChatMessageModel chatMessage);
}