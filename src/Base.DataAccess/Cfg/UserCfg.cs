using Base.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataAccess.Cfg;

/// <summary>
/// Configuration for the user table
/// </summary>
internal class UserCfg : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasAlternateKey(x => x.Login);

        builder.HasMany(user => user.Roles)
            .WithOne(role => role.User)
            .HasForeignKey(role => role.UserId);
    }
}