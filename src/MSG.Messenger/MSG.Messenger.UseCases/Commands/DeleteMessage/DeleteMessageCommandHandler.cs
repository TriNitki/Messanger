using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result<MessageModel>>
{
    private readonly IMessageRepository _messageRepository;

    public DeleteMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Result<MessageModel>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null || message.SentBy != request.MemberId || message.IsDeleted)
            return Result<MessageModel>.Invalid("Invalid message identifier.");

        message.IsDeleted = true;
        await _messageRepository.UpdateAsync(message);
        return Result<MessageModel>.Success(message);
    }
}