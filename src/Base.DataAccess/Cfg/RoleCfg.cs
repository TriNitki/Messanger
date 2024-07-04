using Base.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataAccess.Cfg;

/// <summary>
/// Configuration for the role table
/// </summary>
internal class RoleCfg : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Name);
    }
}