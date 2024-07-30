using MediatR;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Queries.GetChats;

public class GetChatsQueryHandler : IRequestHandler<GetChatsQuery, Result<List<GetChatsResponse.Chat>>>
{
    private readonly IChatRepository _chatRepository;

    public GetChatsQueryHandler(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<Result<List<GetChatsResponse.Chat>>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _chatRepository.GetByUserIdAsync(request.UserId);
        return Result<List<GetChatsResponse.Chat>>.Success(new GetChatsResponse(chats).Result);
    }
}