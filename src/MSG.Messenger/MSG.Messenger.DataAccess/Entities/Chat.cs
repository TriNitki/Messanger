using System.ComponentModel.DataAnnotations;

namespace MSG.Messenger.DataAccess.Entities;

/// <summary>
/// Chat
/// </summary>
public class Chat
{
    /// <summary>
    /// Chat id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Creator id
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// Chat name
    /// </summary>
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Date time of the chat creation 
    /// </summary>
    public DateTime CreationDt { get; set; }

    /// <summary>
    /// Whether chat is deleted
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Whether chat is direct
    /// </summary>
    public bool IsDirect { get; set; }

    /// <summary>
    /// Chat members
    /// </summary>
    public List<ChatMember> Members { get; set; } = [];

    /// <summary>
    /// Chat messages
    /// </summary>
    public List<ChatMessage> Messages { get; set; } = [];
}