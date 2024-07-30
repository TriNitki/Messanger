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

    public async Task<ChatModel> CreateAsync(ChatModel chat)
    {
        var entity = _mapper.Map<Chat>(chat);
        await _context.Chats.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return _mapper.Map<ChatModel>(entity);
    }

    public async Task UpdateAsync(ChatModel chat)
    {
        var entity = _mapper.Map<Chat>(chat);
        _context.Chats.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
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
            return await CreateAsync(chat);
        
        return _mapper.Map<ChatModel>(entity);
    }

    public async Task<ChatModel?> GetByIdAsync(Guid chatId, bool includeMembers = false, bool ? isDirect = null)
    {
        var entities = _context.Chats.AsQueryable();

        if (includeMembers)
            entities = entities.Include(x => x.Members);

        var chats = await entities
            .AsNoTracking()
            .ToListAsync()
            .ConfigureAwait(false);

        if (isDirect != null)
            chats = chats.Where(x => x.IsDirect == isDirect).ToList();

        var chat = chats.FirstOrDefault(x => x.Id == chatId);
        return _mapper.Map<ChatModel>(chat);
    }

    public async Task<List<ChatModel>> GetByUserIdAsync(Guid memberId, int? fromChat = null, int? toChat = null)
    {
        var entities = _context.Chats
            .Include(x => x.Members)
            .Include(x => x.Messages)
            .Where(x => x.Members.Any(y => y.UserId == memberId))
            .OrderByDescending(x => x.Messages.OrderByDescending(y => y.SendingDt).FirstOrDefault())
            .AsQueryable();

        if (fromChat is not null)
            entities = entities.Skip((int)fromChat);

        if (toChat is not null)
        {
            if (toChat <= fromChat)
                return [];

            entities = entities.Take((int)toChat - (fromChat ?? 0));
        }

        var chats = await entities.ToListAsync();

        return _mapper.Map<List<ChatModel>>(chats);
    }

    public async Task MarkAsDeletedAsync(ChatModel chat, bool deleteMembers = false)
    {
        var entity = _mapper.Map<Chat>(chat);

        if (deleteMembers)
            _context.ChatMembers.RemoveRange(entity.Members);

        entity.IsDeleted = true;

        _context.Chats.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}