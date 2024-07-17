using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.UseCases.Abstractions;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Repositories;

/// <summary>
/// <see cref="IClientRepository"/> implementation 
/// </summary>
public class ClientRepository : IClientRepository
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public ClientRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<AuthClient?> ResolveAsync(string name)
    {
        var entity = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name)
            .ConfigureAwait(false);

        return _mapper.Map<AuthClient?>(entity);
    }

    public async Task<AuthClient?> ResolveAsync(string name, string secret)
    {
        var entity = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name && x.HashedSecret == secret)
            .ConfigureAwait(false);

        return _mapper.Map<AuthClient?>(entity);
    }

    /// <inheritdoc/>
    public async Task<AuthClient> CreateAsync(AuthClient client)
    {
        var entity = _mapper.Map<Client>(client);
        await _context.Clients.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return _mapper.Map<AuthClient>(entity);
    }
}