using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MSG.Messenger.Core;
using MSG.Messenger.DataAccess.Entities;
using MSG.Messenger.UseCases.Abstractions;

namespace MSG.Messenger.DataAccess.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public ChatRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ChatModel?> CreateAsync(ChatModel chat)
    {
        var entity = _mapper.Map<Chat>(chat);
        await _context.Chats.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return _mapper.Map<ChatModel>(entity);
    }

    public async Task<ChatModel> GetOrCreateDirectAsync(Guid senderId, Guid receiverId)
    {
        var entity = await _context.Chats.Include(x => x.Members)
            .Where(x => x.IsDirect &&
                        x.Members.FirstOrDefault(y => y.UserId == receiverId || y.UserId == senderId) != null)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        var chat = new ChatModel($"{senderId}.{receiverId}", true, [senderId, receiverId]);

        if (entity is null)
        {
            entity = _mapper.Map<Chat>(chat);

            await _context.Chats.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        
        return _mapper.Map<ChatModel>(entity);
    }

    public async Task<bool> LeaveGroupAsync(Guid userId, Guid chatId)
    {
        var entity = await _context.Chats
            .Include(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == chatId && !x.IsDirect);

        if (entity is null)
            return false;

        var members = entity.Members;
        var leaveMember = members.FirstOrDefault(x => x.UserId == userId);

        if (leaveMember is null)
            return false;

        if (members.Count <= 1)
        {
            entity.IsDeleted = true;
            entity.Members.Clear();
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        if (leaveMember.IsAdmin && !members.Any(x => x.IsAdmin && x.UserId != userId))
        {
            var rndMember = members.FindAll(x => x.UserId != userId).MinBy(x => x.UserId);
            if (rndMember is null)
                return false;

            rndMember.IsAdmin = true;
        }

        _context.ChatMembers.Remove(leaveMember);
        await _context.SaveChangesAsync().ConfigureAwait(false);
        return true;
    }
}