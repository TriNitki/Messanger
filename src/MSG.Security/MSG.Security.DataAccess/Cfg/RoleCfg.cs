using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Cfg;

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