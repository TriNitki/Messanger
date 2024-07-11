using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Cfg;

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