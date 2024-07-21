using MediatR;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.LeaveGroupChat;

public class LeaveGroupChatCommandHandler : IRequestHandler<LeaveGroupChatCommand, Result<Unit>>
{
    private readonly IChatRepository _chatRepository;

    public LeaveGroupChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<Result<Unit>> Handle(LeaveGroupChatCommand request, CancellationToken cancellationToken)
    {
        var isLeft = await _chatRepository.LeaveGroupAsync(request.UserId, request.ChatId);

        return isLeft
            ? Result<Unit>.NoContent()
            : Result<Unit>.Invalid("User was unable to leave the group chat");
    }
}