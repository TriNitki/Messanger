using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.AddMember;

public class AddMemberCommandHandler : AdminBasedHandler, IRequestHandler<AddMemberCommand, Result<ChatMemberModel>>
{
    private readonly IChatMemberRepository _chatMemberRepository;

    public AddMemberCommandHandler(IChatMemberRepository chatMemberRepository)
    : base(chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<ChatMemberModel>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<ChatMemberModel>.Invalid("User is not the admin of this group chat");

        if (await _chatMemberRepository.GetAsync(request.MemberId, request.ChatId) is not null)
            return Result<ChatMemberModel>.Invalid("User is already a member of the group");

        var member = new ChatMemberModel(request.MemberId, request.ChatId);

        await _chatMemberRepository.AddAsync(member);
        return Result<ChatMemberModel>.Created(member);
    }
}