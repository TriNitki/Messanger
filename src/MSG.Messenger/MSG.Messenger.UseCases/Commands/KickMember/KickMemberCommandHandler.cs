using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.KickMember;

public class KickMemberCommandHandler : AdminBasedHandler, IRequestHandler<KickMemberCommand, Result<Unit>>
{
    private readonly IChatMemberRepository _chatMemberRepository;

    public KickMemberCommandHandler(IChatMemberRepository chatMemberRepository)
    : base(chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<Unit>> Handle(KickMemberCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<Unit>.Invalid("User is not the admin of this group chat");

        var kickedMember = await _chatMemberRepository.GetAsync(request.KickedUserId, request.ChatId);

        if (kickedMember is null)
            return Result<Unit>.NoContent();

        if (kickedMember.UserId == request.AdminId)
            return Result<Unit>.Invalid("User cannot kick himself from the chat");


        await _chatMemberRepository.DeleteMemberAsync(kickedMember);
        return Result<Unit>.NoContent();
    }
}