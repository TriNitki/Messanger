using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.EditAdmin;

public class EditAdminCommandHandler : AdminBasedHandler, IRequestHandler<EditAdminCommand, Result<ChatMemberModel>>
{
    private readonly IChatMemberRepository _chatMemberRepository;

    public EditAdminCommandHandler(IChatMemberRepository chatMemberRepository)
    : base(chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<ChatMemberModel>> Handle(EditAdminCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<ChatMemberModel>.Invalid("User is not the admin of this group chat");

        var member = await _chatMemberRepository.GetAsync(request.MemberId, request.ChatId);
        if (member is null)
            return Result<ChatMemberModel>.Invalid("User is not the member of this group chat");

        member.IsAdmin = request.IsAdmin;
        await _chatMemberRepository.UpdateAsync(member);
        return Result<ChatMemberModel>.Success(member);
    }
}