using MediatR;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.KickMember;

public class KickMemberCommand : IRequest<Result<Unit>>
{
    public Guid ChatId { get; set; }

    public Guid AdminId { get; set; }

    public Guid KickedUserId { get; set; }

    public KickMemberCommand(Guid chatId, Guid adminId, Guid kickedUserId)
    {
        ChatId = chatId;
        AdminId = adminId;
        KickedUserId = kickedUserId;
    }
}