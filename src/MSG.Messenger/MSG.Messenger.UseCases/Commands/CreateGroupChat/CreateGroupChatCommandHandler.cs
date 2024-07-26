using MediatR;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.CreateGroupChat;

public class CreateGroupChatCommandHandler : IRequestHandler<CreateGroupChatCommand, Result<ChatModelResult>>
{
    private readonly IChatRepository _chatRepository;

    public CreateGroupChatCommandHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<Result<ChatModelResult>> Handle(CreateGroupChatCommand request, CancellationToken cancellationToken)
    {
        var chat = new ChatModel(request.Name, false, request.Members.ToList(), request.CreatorId);
        var result = await _chatRepository.CreateAsync(chat);

        return Result<ChatModelResult>.Created(result.ToResult());
    }
}