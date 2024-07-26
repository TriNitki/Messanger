using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.GetOrCreateDirectChat;

public class GetOrCreateDirectChatCommandHandler : IRequestHandler<GetOrCreateDirectChatCommand, Result<ChatModelResult>>
{
    private readonly IChatRepository _chatRepository;

    public GetOrCreateDirectChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<Result<ChatModelResult>> Handle(GetOrCreateDirectChatCommand request, CancellationToken cancellationToken)
    {
        if (request.Sender == request.Receiver)
            return Result<ChatModelResult>.Invalid("Unable to create a chat with yourself");

        var result = await _chatRepository.GetOrCreateDirectAsync(request.Sender, request.Receiver);

        return Result<ChatModelResult>.Success(result.ToResult());
    }
}