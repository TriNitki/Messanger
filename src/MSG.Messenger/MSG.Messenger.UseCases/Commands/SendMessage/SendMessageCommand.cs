using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.SendMessage;

public class SendMessageCommand : IRequest<Result<ChatMessageModel>>
{
    public Guid ChatId { get; set; }

    public Guid MemberId { get; set; }

    public string Message { get; set; }

    public SendMessageCommand(Guid chatId, Guid memberId, string message)
    {
        ChatId = chatId;
        MemberId = memberId;
        Message = message;
    }
}