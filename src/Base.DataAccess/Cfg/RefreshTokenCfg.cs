using Base.Authentication.Core;
using Base.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataAccess.Cfg;

/// <summary>
/// Configuration for the refresh token table
/// </summary>
internal class RefreshTokenCfg : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Token);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);
    }
}