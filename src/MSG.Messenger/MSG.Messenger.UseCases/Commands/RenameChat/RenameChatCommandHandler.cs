using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.RenameChat;

public class RenameChatCommandHandler : AdminBasedHandler, IRequestHandler<RenameChatCommand, Result<ChatModelResult>>
{
    private readonly IChatRepository _chatRepository;

    public RenameChatCommandHandler(IChatRepository chatRepository, IChatMemberRepository chatMemberRepository)
    : base(chatMemberRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<Result<ChatModelResult>> Handle(RenameChatCommand request, CancellationToken cancellationToken)
    {
        if (!await IsAdmin(request.ChatId, request.AdminId))
            return Result<ChatModelResult>.Invalid("User is not the admin of this group chat");

        var chat = await _chatRepository.GetByIdAsync(request.ChatId);

        if (chat is null)
            return Result<ChatModelResult>.Invalid("Chat do not exist");

        chat.Name = request.NewName;

        await _chatRepository.UpdateAsync(chat);
        return Result<ChatModelResult>.Success(chat.ToResult());
    }
}