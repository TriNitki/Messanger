using Base.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataAccess.Cfg;

/// <summary>
/// Configuration for the role to user table
/// </summary>
internal class RoleToUserCfg : IEntityTypeConfiguration<RoleToUser>
{
    public void Configure(EntityTypeBuilder<RoleToUser> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.UserId });

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}