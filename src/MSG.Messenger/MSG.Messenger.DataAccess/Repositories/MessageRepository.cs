using AutoMapper;
using MSG.Messenger.Core;
using MSG.Messenger.UseCases.Abstractions;
using Microsoft.EntityFrameworkCore;
using MSG.Messenger.DataAccess.Entities;

namespace MSG.Messenger.DataAccess.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public MessageRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MessageModel>> GetByChatIdAsync(
        Guid chatId, int? fromMessage = null, int? toMessage = null, bool sortByNewest = true, bool excludeDeleted = true)
    {
        var entities = _context.Messages
            .Where(x => x.ChatId == chatId)
            .AsQueryable();

        if (excludeDeleted)
            entities = entities.Where(x => !x.IsDeleted);

        if (sortByNewest)
            entities = entities.OrderByDescending(x => x.SendingDt);

        if (fromMessage is not null)
            entities = entities.Skip((int)fromMessage);

        if (toMessage is not null)
        {
            if (toMessage <= fromMessage)
                return [];

            entities = entities.Take((int)toMessage - (fromMessage ?? 0));
        }

        var messages = await entities.AsNoTracking().ToListAsync().ConfigureAwait(false);
        return _mapper.Map<List<MessageModel>>(messages);
    }

    public async Task<MessageModel?> GetByIdAsync(Guid messageId)
    {
        var entity = await _context.Messages.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == messageId)
            .ConfigureAwait(false);

        return _mapper.Map<MessageModel?>(entity);
    }

    public async Task CreateAsync(MessageModel message)
    {
        var entity = _mapper.Map<Message>(message);
        await _context.Messages.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task UpdateAsync(MessageModel message)
    {
        var entity = _mapper.Map<Message>(message);
        _context.Messages.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}