using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.Core.Abstractions;
using MSG.Security.Authentication.UseCases.Abstractions;
using MSG.Security.Service.Options;

namespace MSG.Security.Service.Infrastructure;

/// <inheritdoc/>
public class TokenService : ITokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenFamilyRepository _refreshTokenFamilyRepository;
    private readonly SecurityOptions _options;

    public TokenService(
        IOptions<SecurityOptions> options, 
        IRefreshTokenRepository refreshTokenRepository, 
        IRefreshTokenFamilyRepository refreshTokenFamilyRepository)
    {
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _refreshTokenFamilyRepository = refreshTokenFamilyRepository ?? throw new ArgumentNullException(nameof(refreshTokenFamilyRepository));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc/>
    public Task<string> GenerateAccessToken(IAuthenticatedCustomer customer)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeInMinutes);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: customer.GetClaims(),
            notBefore: null,
            expires: expiration,
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(tokenValue);
    }

    /// <inheritdoc/>
    public async Task<RefreshToken> GenerateRefreshToken(Guid userId, Guid tokenFamilyId)
    {
        var tokenFamily = await _refreshTokenFamilyRepository.GetOrCreateByIdAsync(tokenFamilyId);

        var expiration = DateTime.UtcNow.AddMinutes(_options.RefreshTokenLifetimeInMinutes);
        var token = new RefreshToken
        {
            Content = Guid.NewGuid().ToString(),
            Expiration = expiration,
            UserId = userId,
            IsUsed = false,
            FamilyId = tokenFamily.Id
        };

        await _refreshTokenRepository.CreateAsync(token);
        return token;
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> UseRefreshToken(string token)
    {
        var tokenEntity = await _refreshTokenRepository.GetByTokenAsync(token);

        if (tokenEntity is null || tokenEntity.Family!.IsLocked)
            return null;

        if (!tokenEntity.IsValid() && !tokenEntity.Family!.IsLocked)
        {
            await _refreshTokenFamilyRepository.LockByIdAsync(tokenEntity.Family);
            return null;
        }

        await _refreshTokenRepository.DeactivateAsync(tokenEntity);
        return tokenEntity;
    }
}