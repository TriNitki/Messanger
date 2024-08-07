using MediatR;
using MSG.Messenger.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.LeaveGroupChat;

public class LeaveGroupChatCommandHandler : IRequestHandler<LeaveGroupChatCommand, Result<LeaveGroupChatResult>>
{
    private readonly IChatRepository _chatRepository;
    private readonly IChatMemberRepository _chatMemberRepository;

    public LeaveGroupChatCommandHandler(IChatRepository chatRepository, IChatMemberRepository chatMemberRepository)
    {
        _chatRepository = chatRepository;
        _chatMemberRepository = chatMemberRepository;
    }

    public async Task<Result<LeaveGroupChatResult>> Handle(LeaveGroupChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatRepository.GetByIdAsync(
            request.ChatId,
            includeMembers: true,
            isDirect: false);

        if (chat is null)
            return Result<LeaveGroupChatResult>.Invalid("Group chat was not found");

        var members = chat.Members;
        var leaveMember = members.FirstOrDefault(x => x.UserId == request.UserId);

        if (leaveMember is null)
            return Result<LeaveGroupChatResult>.Invalid("User is not a member of this group chat");

        if (members.Count <= 1)
        {
            await _chatRepository.MarkAsDeletedAsync(chat, true);
            return Result<LeaveGroupChatResult>.Success(new LeaveGroupChatResult(chat.IgnoreMembers().ToResult()));
        }

        await _chatMemberRepository.DeleteMemberAsync(leaveMember);

        Guid? newAdminId = null;
        if (leaveMember.IsAdmin && !members.Any(x => x.IsAdmin))
        {
            var rndMember = members.FindAll(x => x.UserId != request.UserId).MinBy(x => x.UserId);

            if (rndMember is null)
                throw new ArgumentNullException(nameof(rndMember));

            rndMember.IsAdmin = true;
            await _chatMemberRepository.UpdateAsync(rndMember);
            newAdminId = rndMember.UserId;
        }

        return Result<LeaveGroupChatResult>.Success(new LeaveGroupChatResult(
            chat.IgnoreMembers().ToResult(), newAdminId));
    }
}