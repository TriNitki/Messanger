using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSG.Messenger.DataAccess.Entities;

namespace MSG.Messenger.DataAccess.Cfg;

internal class ChatMemberCfg : IEntityTypeConfiguration<ChatMember>
{
    public void Configure(EntityTypeBuilder<ChatMember> builder)
    {
        builder.HasKey(x => new { x.ChatId, x.UserId });

        builder.HasOne<Chat>()
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.ChatId);
    }
}