using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess.Cfg;

internal class ClientCfg : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.Name);
    }
}