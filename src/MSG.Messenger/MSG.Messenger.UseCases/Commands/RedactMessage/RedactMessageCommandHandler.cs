using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.RedactMessage;

public class RedactMessageCommandHandler : IRequestHandler<RedactMessageCommand, Result<ChatMessageModel>>
{
    private readonly IChatMessageRepository _chatMessageRepository;

    public RedactMessageCommandHandler(IChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }

    public async Task<Result<ChatMessageModel>> Handle(RedactMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _chatMessageRepository.GetByIdAsync(request.MessageId);
        if (message == null || message.SentBy != request.MemberId || message.IsDeleted)
            return Result<ChatMessageModel>.Invalid("This message cannot be redacted");

        message.Content = request.NewContent;
        message.IsRedacted = true;
        await _chatMessageRepository.UpdateAsync(message);
        return Result<ChatMessageModel>.Success(message);
    }
}