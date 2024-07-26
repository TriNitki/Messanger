using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Queries.GetChat;

public class GetChatQuery : IRequest<Result<ChatModelResult>>
{
    public Guid MemberId { get; set; }

    public Guid ChatId { get; set; }

    public int FromMessage { get; set; }

    public int ToMessage { get; set; }

    public GetChatQuery(Guid chatId, Guid memberId, int fromMessage, int toMessage)
    {
        ChatId = chatId;
        MemberId = memberId;
        FromMessage = fromMessage;
        ToMessage = toMessage;
    }
}