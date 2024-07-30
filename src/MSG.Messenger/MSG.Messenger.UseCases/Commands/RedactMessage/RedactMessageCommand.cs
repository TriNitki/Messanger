using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.RedactMessage;

public class RedactMessageCommand : IRequest<Result<MessageModel>>
{
    public Guid MessageId { get; set; }

    public Guid MemberId { get; set; }

    public string NewContent { get; set; }

    public RedactMessageCommand(Guid messageId, Guid memberId, string newContent)
    {
        MessageId = messageId;
        MemberId = memberId;
        NewContent = newContent;
    }
}