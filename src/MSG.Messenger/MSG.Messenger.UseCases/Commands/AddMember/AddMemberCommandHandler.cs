using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.AddMember;

public class AddMemberCommandHandler : AdminBasedHandler, IRequestHandler<AddMemberCommand, Result<ChatModelResult>>
{
    private readonly IChatMemberRepository _chatMemberRepository;
    private readonly IChatRepository _chatRepository;

    public AddMemberCommandHandler(IChatMemberRepository chatMemberRepository, IChatRepository chatRepository)
    : base(chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
        _chatRepository = chatRepository;
    }

    public async Task<Result<ChatModelResult>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<ChatModelResult>.Invalid("User is not the admin of this group chat");

        var chat = await _chatRepository.GetByIdAsync(request.ChatId, true, false);

        if (chat is null)
            return Result<ChatModelResult>.Invalid("Group chat was not found");

        if (chat.Members.Find(x => x.UserId == request.MemberId) is not null)
            return Result<ChatModelResult>.Invalid("User is already a member of the group");

        await _chatMemberRepository.AddAsync(new ChatMemberModel(request.MemberId, request.ChatId));
        return Result<ChatModelResult>.Success(chat.IgnoreMembers().ToResult());
    }
}