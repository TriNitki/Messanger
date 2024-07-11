using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Cfg;

/// <summary>
/// Configuration for the role to feature table
/// </summary>
internal class RoleToFeatureCfg : IEntityTypeConfiguration<RoleToFeature>
{
    public void Configure(EntityTypeBuilder<RoleToFeature> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.FeatureId });

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId);
    }
}