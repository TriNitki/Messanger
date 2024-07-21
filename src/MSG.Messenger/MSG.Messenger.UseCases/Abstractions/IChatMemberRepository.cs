using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

public interface IChatMemberRepository
{
    public Task UpdateAsync(ChatMemberModel member);

    public Task DeleteMemberAsync(ChatMemberModel member);
}