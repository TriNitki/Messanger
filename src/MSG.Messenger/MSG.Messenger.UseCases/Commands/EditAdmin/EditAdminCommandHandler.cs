using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.EditAdmin;

public class EditAdminCommandHandler : AdminBasedHandler, IRequestHandler<EditAdminCommand, Result<ChatModelResult>>
{
    private readonly IChatMemberRepository _chatMemberRepository;
    private readonly IChatRepository _chatRepository;

    public EditAdminCommandHandler(IChatMemberRepository chatMemberRepository, IChatRepository chatRepository)
    : base(chatMemberRepository)
    {
        _chatMemberRepository = chatMemberRepository;
        _chatRepository = chatRepository;
    }

    public async Task<Result<ChatModelResult>> Handle(EditAdminCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<ChatModelResult>.Invalid("User is not the admin of this group chat");

        var chat = await _chatRepository.GetByIdAsync(request.ChatId, true, false);

        if (chat is null)
            return Result<ChatModelResult>.Invalid("Wrong group chat id");

        var member = chat.Members.Find(x => x.UserId == request.MemberId);
        if (member is null)
            return Result<ChatModelResult>.Invalid("User is not the member of this group chat");

        member.IsAdmin = request.IsAdmin;
        await _chatMemberRepository.UpdateAsync(member);
        return Result<ChatModelResult>.Success(chat.IgnoreMembers().ToResult());
    }
}