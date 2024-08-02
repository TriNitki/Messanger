namespace MSG.Messenger.Core;

/// <summary>
/// Chat model
/// </summary>
public class ChatModel
{
    /// <summary>
    /// New chat constructor
    /// </summary>
    /// <param name="name"> Chat name </param>
    /// <param name="isDirect"> Whether the chat is direct (or group) </param>
    /// <param name="members"> Chat members </param>
    /// <param name="creatorId"> Creator id </param>
    public ChatModel(string name, bool isDirect, List<ChatMemberModel> members, Guid creatorId)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreationDt = DateTime.UtcNow;
        IsDirect = isDirect;
        IsDeleted = false;
        Members = members;
        CreatorId = creatorId;
        Messages = [];
    }

    /// <summary>
    /// New chat constructor
    /// </summary>
    /// <param name="name"> Chat name </param>
    /// <param name="isDirect"> Whether the chat is direct </param>
    /// <param name="members"> List of member ids </param>
    /// <param name="creatorId"> Creator id </param>
    public ChatModel(string name, bool isDirect, List<Guid> members, Guid? creatorId = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreationDt = DateTime.UtcNow;
        IsDirect = isDirect;
        IsDeleted = false;
        Members = members.Select(userId => new ChatMemberModel(userId, Id)).ToList();
        CreatorId = creatorId;
        Messages = [];

        if (creatorId != null)
            Members.ForEach(x => x.IsAdmin = x.UserId == creatorId);
    }

    /// <summary>
    /// Existing chat constructor
    /// </summary>
    /// <param name="id"> Chat id </param>
    /// <param name="creatorId"> Creator id </param>
    /// <param name="name"> Chat name </param>
    /// <param name="creationDt"> Creation date time of the chat </param>
    /// <param name="isDeleted"> Whether the chat is deleted </param>
    /// <param name="isDirect"> Whether the chat is direct (or group) </param>
    /// <param name="members"> Chat members </param>
    /// <param name="messages"> Chat messages </param>
    public ChatModel(Guid id, Guid creatorId, string name, DateTime creationDt, bool isDeleted, bool isDirect,
        List<ChatMemberModel> members, List<MessageModel> messages)
    {
        Id = id;
        CreatorId = creatorId;
        Name = name;
        CreationDt = creationDt;
        IsDeleted = isDeleted;
        IsDirect = isDirect;
        Members = members;
        Messages = messages;
    }

    /// <summary>
    /// Retrieves an object without repeating data
    /// </summary>
    /// <returns> Object without repeating data </returns>
    public ChatModelResult ToResult()
    {
        return new ChatModelResult(
            Id, CreatorId, Name, CreationDt, IsDeleted, IsDirect,
            Members.Select(x => 
                new ChatModelResultMember(x.UserId, x.IsAdmin)).ToList(),
            Messages.Select(x =>
                new ChatModelResultMessage(x.Id, x.Content, x.SentBy, x.SendingDt, x.IsRedacted, x.IsDeleted)).ToList());
    }

    /// <summary>
    /// Chat id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Creator id
    /// </summary>
    public Guid? CreatorId { get; }

    /// <summary>
    /// Chat name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Date time of the chat creation 
    /// </summary>
    public DateTime CreationDt { get; }

    /// <summary>
    /// Whether chat is deleted
    /// </summary>
    public bool IsDeleted { get; }

    /// <summary>
    /// Whether chat is direct
    /// </summary>
    public bool IsDirect { get; }

    /// <summary>
    /// List of chat members
    /// </summary>
    public List<ChatMemberModel> Members { get; }

    /// <summary>
    /// List of messages
    /// </summary>
    public List<MessageModel> Messages { get; set; }
}

public record ChatModelResult(
    Guid Id, Guid? CreatorId, string Name, DateTime CreationDt, bool IsDeleted, bool IsDirect, List<ChatModelResultMember> Members, List<ChatModelResultMessage> Messages);

public record ChatModelResultMember(Guid UserId, bool IsAdmin);

public record ChatModelResultMessage(Guid Id, string Content, Guid SentBy, DateTime SendingDt, bool IsRedacted, bool IsDeleted);