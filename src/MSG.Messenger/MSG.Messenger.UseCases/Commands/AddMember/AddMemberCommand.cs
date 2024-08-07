using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.AddMember;

public class AddMemberCommand : IRequest<Result<ChatModelResult>>
{
    public Guid AdminId { get; set; }

    public Guid MemberId { get; set; }

    public Guid ChatId { get; set; }

    public AddMemberCommand(Guid chatId, Guid adminId, Guid memberId)
    {
        MemberId = memberId;
        ChatId = chatId;
        AdminId = adminId;
    }
}