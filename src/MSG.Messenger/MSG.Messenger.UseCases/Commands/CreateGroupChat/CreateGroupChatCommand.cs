using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.CreateGroupChat;

public class CreateGroupChatCommand : IRequest<Result<ChatModel>>
{
    public string Name { get; set; }

    public Guid CreatorId { get; set; }

    public HashSet<Guid> Members { get; set; }

    public CreateGroupChatCommand(string name, HashSet<Guid> members, Guid creatorId)
    {
        Name = name;
        Members = members;
        CreatorId = creatorId;
    }
}