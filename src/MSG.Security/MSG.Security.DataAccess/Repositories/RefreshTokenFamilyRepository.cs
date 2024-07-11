using Microsoft.EntityFrameworkCore;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.UseCases.Abstractions;

namespace MSG.Security.DataAccess.Repositories;

public class RefreshTokenFamilyRepository : IRefreshTokenFamilyRepository
{
    private readonly DataBaseContext _context;

    public RefreshTokenFamilyRepository(DataBaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<RefreshTokenFamily> GetOrCreateByIdAsync(Guid id)
    {
        var entity = await _context.RefreshTokenFamilies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id)
            .ConfigureAwait(false);

        if (entity != null) 
            return entity;

        var createdEntity = await _context.RefreshTokenFamilies.AddAsync(new RefreshTokenFamily()
        {
            Id = id,
            IsLocked = false
        });
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return createdEntity.Entity;
    }

    public async Task LockByIdAsync(RefreshTokenFamily family)
    {
        family.Lock();
        _context.RefreshTokenFamilies.Update(family);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}