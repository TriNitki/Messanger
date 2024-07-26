using MediatR;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Queries.GetChats;

public class GetChatsQuery : IRequest<Result<List<GetChatsResponse.Chat>>>
{
    public Guid UserId { get; set; }

    public int FromChat { get; set; }

    public int ToChat { get; set; }

    public GetChatsQuery(Guid userId, int fromChat, int toChat)
    {
        UserId = userId;
        FromChat = fromChat;
        ToChat = toChat;
    }
}