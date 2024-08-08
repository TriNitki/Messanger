using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Messenger.DataAccess.Entities;

namespace MSG.Messenger.DataAccess.Cfg;

internal class ChatMessageCfg : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasOne<Chat>()
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId);
    }
}