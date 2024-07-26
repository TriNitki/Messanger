using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.RenameChat;

public class RenameChatCommand : IRequest<Result<ChatModelResult>>
{
    public Guid AdminId { get; set; }

    public Guid ChatId { get; set; }

    public string NewName { get; set; }

    public RenameChatCommand(Guid chatId, Guid adminId, string newName)
    {
        AdminId = adminId;
        ChatId = chatId;
        NewName = newName;
    }
}