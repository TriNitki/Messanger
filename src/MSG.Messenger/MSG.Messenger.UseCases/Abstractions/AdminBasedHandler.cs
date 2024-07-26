namespace MSG.Messenger.UseCases.Abstractions;

public abstract class AdminBasedHandler
{
    private readonly IChatMemberRepository _chatMemberRepository;

    protected internal AdminBasedHandler(IChatMemberRepository chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
    }

    protected async Task<bool> IsAdmin(Guid chatId, Guid memberId)
    {
        var adminMember = await _chatMemberRepository.GetAsync(memberId,chatId);
        return adminMember is not null && adminMember.IsAdmin;
    }
}