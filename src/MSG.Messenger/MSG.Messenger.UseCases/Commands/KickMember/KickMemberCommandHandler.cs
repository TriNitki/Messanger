using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.KickMember;

public class KickMemberCommandHandler : AdminBasedHandler, IRequestHandler<KickMemberCommand, Result<ChatModelResult>>
{
    private readonly IChatRepository _chatRepository;

    private readonly IChatMemberRepository _chatMemberRepository;

    public KickMemberCommandHandler(IChatMemberRepository chatMemberRepository, IChatRepository chatRepository)
    : base(chatMemberRepository)
    {
        _chatRepository = chatRepository;
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<ChatModelResult>> Handle(KickMemberCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<ChatModelResult>.Invalid("User is not the admin of this group chat");

        if (request.KickedUserId == request.AdminId)
            return Result<ChatModelResult>.Invalid("User cannot kick himself from the chat");

        var chat = await _chatRepository.GetByIdAsync(request.ChatId, true, false);

        if (chat is null)
            return Result<ChatModelResult>.Invalid("Wrong group chat id");

        var kickedMember = chat.Members.Find(x => x.UserId == request.KickedUserId);

        if (kickedMember is null)
            return Result<ChatModelResult>.Invalid("User is not a member of the group chat.");

        await _chatMemberRepository.DeleteMemberAsync(kickedMember);
        return Result<ChatModelResult>.Success(chat.IgnoreMembers().ToResult());
    }
}