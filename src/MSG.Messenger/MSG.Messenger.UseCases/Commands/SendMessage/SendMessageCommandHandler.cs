using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<MessageModel>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChatMemberRepository _chatMemberRepository;

    public SendMessageCommandHandler(IMessageRepository messageRepository, IChatMemberRepository chatMemberRepository)
    {
        _messageRepository = messageRepository;
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<MessageModel>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (await _chatMemberRepository.GetAsync(request.MemberId, request.ChatId) is null)
            return Result<MessageModel>.Invalid("User is not a member of this chat");

        var message = new MessageModel(request.ChatId, request.MemberId, request.Message);
        await _messageRepository.CreateAsync(message);

        return Result<MessageModel>.Created(message);
    }
}