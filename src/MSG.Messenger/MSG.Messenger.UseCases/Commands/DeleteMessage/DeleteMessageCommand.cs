using MediatR;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.DeleteMessage;

public class DeleteMessageCommand : IRequest<Result<Unit>>  
{
    public Guid MessageId { get; set; }

    public Guid MemberId { get; set; }

    public DeleteMessageCommand(Guid messageId, Guid memberId)
    {
        MessageId = messageId;
        MemberId = memberId;
    }
}