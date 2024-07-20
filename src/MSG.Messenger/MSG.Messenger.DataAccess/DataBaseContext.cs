using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MSG.Messenger.DataAccess.Entities;

namespace MSG.Messenger.DataAccess;

/// <summary>
/// Data base context
/// </summary>
/// <param name="options"> Options to be used by DbContext </param>
public class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    internal DbSet<Chat> Chats { get; set; }

    internal DbSet<Message> Messages { get; set; }

    internal DbSet<ChatMember> ChatMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}