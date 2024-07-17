using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.UseCases.Abstractions;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Repositories;

/// <summary>
/// <see cref="IUserRepository"/> implementation 
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DataBaseContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<AuthUser?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        return _mapper.Map<AuthUser?>(entity);
    }

    /// <inheritdoc/>
    public async Task<AuthUser?> ResolveAsync(string login)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Login == login)
            .ConfigureAwait(false);

        return _mapper.Map<AuthUser?>(entity);
    }

    /// <inheritdoc/>
    public async Task<AuthUser?> ResolveAsync(string login, string password)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Login == login && x.HashedPassword == password)
            .ConfigureAwait(false);

        return _mapper.Map<AuthUser?>(entity);
    }

    /// <inheritdoc/>
    public async Task<AuthUser> CreateAsync(AuthUser user)
    {
        var entity = _mapper.Map<User>(user);
        await _context.Users.AddAsync(entity).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return _mapper.Map<AuthUser>(entity);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(AuthUser user)
    {
        var entity = _mapper.Map<User>(user);
        _context.Users.Update(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}