using MSG.Messenger.Core;

namespace MSG.Messenger.UseCases.Abstractions;

/// <summary>
/// Chat member repository interface
/// </summary>
public interface IChatMemberRepository
{
    /// <summary>
    /// Add new chat member
    /// </summary>
    /// <param name="chatMember"> Chat member model </param>
    public Task AddAsync(ChatMemberModel chatMember);

    /// <summary>
    /// Update existing chat member
    /// </summary>
    /// <param name="chatMember"> Chat member model </param>
    public Task UpdateAsync(ChatMemberModel chatMember);

    /// <summary>
    /// Delete chat member
    /// </summary>
    /// <param name="chatMember"> Chat member model </param>
    public Task DeleteMemberAsync(ChatMemberModel chatMember);

    /// <summary>
    /// Get chat member
    /// </summary>
    /// <param name="memberId"> Member id </param>
    /// <param name="chatId"> Chat id </param>
    /// <returns> Chat member model </returns>
    public Task<ChatMemberModel?> GetAsync(Guid memberId, Guid chatId);
}