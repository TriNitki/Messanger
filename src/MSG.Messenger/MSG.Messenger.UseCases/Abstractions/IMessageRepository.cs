using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

public interface IMessageRepository
{
    public Task<List<MessageModel>> GetByChatIdAsync(
        Guid chatId, int? fromMessage = null, int? toMessage = null, bool sortByNewest = true, bool excludeDeleted = true);

    public Task<MessageModel?> GetByIdAsync(Guid messageId);

    public Task CreateAsync(MessageModel message);

    public Task UpdateAsync(MessageModel message);
}