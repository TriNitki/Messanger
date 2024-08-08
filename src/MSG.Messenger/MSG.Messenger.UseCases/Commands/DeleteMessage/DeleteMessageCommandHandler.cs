using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result<ChatMessageModel>>
{
    private readonly IChatMessageRepository _chatMessageRepository;

    public DeleteMessageCommandHandler(IChatMessageRepository chatMessageRepository)
    {
        _chatMessageRepository = chatMessageRepository;
    }

    public async Task<Result<ChatMessageModel>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _chatMessageRepository.GetByIdAsync(request.MessageId);
        if (message == null || message.SentBy != request.MemberId || message.IsDeleted)
            return Result<ChatMessageModel>.Invalid("Invalid message identifier.");

        message.IsDeleted = true;
        await _chatMessageRepository.UpdateAsync(message);
        return Result<ChatMessageModel>.Success(message);
    }
}