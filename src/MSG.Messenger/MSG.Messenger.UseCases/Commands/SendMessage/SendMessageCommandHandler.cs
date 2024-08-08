using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.SendMessage;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<ChatMessageModel>>
{
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IChatMemberRepository _chatMemberRepository;

    public SendMessageCommandHandler(IChatMessageRepository chatMessageRepository, IChatMemberRepository chatMemberRepository)
    {
        _chatMessageRepository = chatMessageRepository;
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<ChatMessageModel>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        if (await _chatMemberRepository.GetAsync(request.MemberId, request.ChatId) is null)
            return Result<ChatMessageModel>.Invalid("User is not a member of this chat");

        var message = new ChatMessageModel(request.ChatId, request.MemberId, request.Message);
        await _chatMessageRepository.CreateAsync(message);

        return Result<ChatMessageModel>.Created(message);
    }
}