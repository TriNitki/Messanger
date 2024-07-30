namespace MSG.Messenger.Core;

/// <summary>
/// Message model
/// </summary>
public class MessageModel
{
    /// <summary>
    /// New message constructor
    /// </summary>
    /// <param name="chatId"> Chat id </param>
    /// <param name="sentBy"> Member id who sent the message </param>
    /// <param name="content"> Content of the message </param>
    public MessageModel(Guid chatId, Guid sentBy, string content)
    {
        Id = Guid.NewGuid();
        ChatId = chatId;
        SentBy = sentBy;
        Content = content;
        SendingDt = DateTime.UtcNow;
        IsRedacted = false;
        IsDeleted = false;
    }

    /// <summary>
    /// Existing message constructor
    /// </summary>
    /// <param name="id"> Message id </param>
    /// <param name="chatId"> Chat id </param>
    /// <param name="sentBy"> Member id who sent the message </param>
    /// <param name="content"> Content of the message </param>
    /// <param name="sendingDt"> Date time when the message was sent </param>
    /// <param name="isRedacted"> Whether the message is redacted </param>
    /// <param name="isDeleted"> Whether the message is deleted </param>
    public MessageModel(Guid id, Guid chatId, Guid sentBy, string content, DateTime sendingDt, bool isRedacted, bool isDeleted)
    {
        Id = id;
        ChatId = chatId;
        SentBy = sentBy;
        Content = content;
        SendingDt = sendingDt;
        IsRedacted = isRedacted;
        IsDeleted = isDeleted;
    }

    /// <summary>
    /// Message id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Chat id to which the message was sent
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// Message content
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Member id who sent the message
    /// </summary>
    public Guid SentBy { get; set; }

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