using System.ComponentModel.DataAnnotations.Schema;

namespace MSG.Messenger.DataAccess.Entities;

/// <summary>
/// Message
/// </summary>
public class Message
{
    /// <summary>
    /// Message id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Chat id to which the message was sent
    /// </summary>
    public Guid ChatId { get; set; }

    [Column(TypeName = "text")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Date time when the message was sent
    /// </summary>
    public DateTime SendingDt { get; set; }

    /// <summary>
    /// Whether the message is redacted
    /// </summary>
    public bool IsRedacted { get; set; }

    /// <summary>
    /// Whether the message is deleted
    /// </summary>
    public bool IsDeleted { get; set; }
}