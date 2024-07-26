using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.RedactMessage;

public class RedactMessageCommandHandler : IRequestHandler<RedactMessageCommand, Result<MessageModel>>
{
    private readonly IMessageRepository _messageRepository;

    public RedactMessageCommandHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public async Task<Result<MessageModel>> Handle(RedactMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null || message.SentBy != request.MemberId || message.IsDeleted)
            return Result<MessageModel>.Invalid("This message cannot be redacted");

        message.Content = request.NewContent;
        message.IsRedacted = true;
        await _messageRepository.UpdateAsync(message);
        return Result<MessageModel>.Success(message);
    }
}