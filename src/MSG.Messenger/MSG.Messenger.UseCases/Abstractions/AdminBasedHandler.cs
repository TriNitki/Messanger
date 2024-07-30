namespace MSG.Messenger.UseCases.Abstractions;

/// <summary>
/// Abstract class for handlers that require validation to chat admin rights
/// </summary>
public abstract class AdminBasedHandler
{
    private readonly IChatMemberRepository _chatMemberRepository;

    protected internal AdminBasedHandler(IChatMemberRepository chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
    }

    /// <summary>
    /// Check whether the user is admin in the group chat
    /// </summary>
    /// <param name="chatId"> Chat id </param>
    /// <param name="memberId"> Member id </param>
    /// <returns> <see langword="true"/> if the user is admin, otherwise <see langword="false"/> </returns>
    protected async Task<bool> IsAdmin(Guid chatId, Guid memberId)
    {
        var adminMember = await _chatMemberRepository.GetAsync(memberId,chatId);
        return adminMember is not null && adminMember.IsAdmin;
    }
}