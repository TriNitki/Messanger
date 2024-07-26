using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

public interface IChatMemberRepository
{
    public Task AddAsync(ChatMemberModel member);

    public Task UpdateAsync(ChatMemberModel member);

    public Task DeleteMemberAsync(ChatMemberModel member);

    public Task<ChatMemberModel?> GetAsync(Guid memberId, Guid chatId);
}