using Base.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MSG.Security.DataAccess.Cfg;

/// <summary>
/// Configuration for the feature table
/// </summary>
internal class FeatureCfg : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.HasKey(x => x.Name);

        builder.HasMany(x => x.Roles)
            .WithOne(x => x.Feature)
            .HasForeignKey(x => x.FeatureId);
    }
}