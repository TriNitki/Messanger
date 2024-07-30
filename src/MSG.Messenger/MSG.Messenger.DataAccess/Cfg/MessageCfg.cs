using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Messenger.DataAccess.Entities;

namespace MSG.Messenger.DataAccess.Cfg;

internal class MessageCfg : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne<Chat>()
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId);
    }
}