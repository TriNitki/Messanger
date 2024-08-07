using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.EditAdmin;

public class EditAdminCommand : IRequest<Result<ChatModelResult>>
{
    public Guid AdminId { get; set; }

    public Guid MemberId { get; set; }

    public Guid ChatId { get; set; }

    public bool IsAdmin { get; set; }

    public EditAdminCommand(Guid chatId, Guid adminId, Guid memberId, bool isAdmin)
    {
        ChatId = chatId;
        AdminId = adminId;
        MemberId = memberId;
        IsAdmin = isAdmin;
    }
}