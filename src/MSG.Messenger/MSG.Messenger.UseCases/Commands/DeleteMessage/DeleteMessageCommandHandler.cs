using MediatR;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result<Unit>>
{
    private readonly IMessageRepository _messageRepository;

    public DeleteMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Result<Unit>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null || message.SentBy != request.MemberId || message.IsDeleted)
            return Result<Unit>.Invalid("Invalid message identifier.");

        message.IsDeleted = true;
        await _messageRepository.UpdateAsync(message);
        return Result<Unit>.NoContent();
    }
}