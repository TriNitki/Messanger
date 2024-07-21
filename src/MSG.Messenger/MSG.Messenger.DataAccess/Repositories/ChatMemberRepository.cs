using AutoMapper;
using MSG.Messenger.Core;
using MSG.Messenger.DataAccess.Entities;
using MSG.Messenger.UseCases.Abstractions;

namespace MSG.Messenger.DataAccess.Repositories;

public class ChatMemberRepository : IChatMemberRepository
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public ChatMemberRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task UpdateAsync(ChatMemberModel member)
    {
        var entity = _mapper.Map<ChatMember>(member);
        _context.ChatMembers.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);

    }

    public async Task DeleteMemberAsync(ChatMemberModel member)
    {
        var entity = _mapper.Map<ChatMember>(member);
        _context.ChatMembers.Remove(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}