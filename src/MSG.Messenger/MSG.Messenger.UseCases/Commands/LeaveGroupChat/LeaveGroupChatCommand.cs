using MediatR;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.LeaveGroupChat;

public class LeaveGroupChatCommand : IRequest<Result<Unit>>
{
    public Guid UserId { get; set; }

    public Guid ChatId { get; set; }

    public LeaveGroupChatCommand(Guid userId, Guid chatId)
    {
        UserId = userId;
        ChatId = chatId;
    }
}