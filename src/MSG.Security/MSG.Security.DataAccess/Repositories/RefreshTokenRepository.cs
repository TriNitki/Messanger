using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.UseCases.Abstractions;

namespace MSG.Security.DataAccess.Repositories;

/// <summary>
/// <see cref="IRefreshTokenRepository"/> implementation
/// </summary>
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DataBaseContext _context;

    public RefreshTokenRepository(DataBaseContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        var entity = await _context.RefreshTokens
            .AsNoTracking()
            .Include(x => x.Family)
            .FirstOrDefaultAsync(x => x.Content == token)
            .ConfigureAwait(false);

        return entity;
    }

    /// <inheritdoc/>
    public async Task CreateAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeactivateAsync(RefreshToken token)
    {
        token.Deactivate();
        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeactivateAllTokensAsync(Guid userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(x => x.UserId == userId)
            .ToListAsync()
            .ConfigureAwait(false);

        tokens.ForEach(x => x.Deactivate());
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}