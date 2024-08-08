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
        List<ChatMemberModel> members, List<ChatMessageModel> messages)
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
    /// Returns the instance of the chat class in which the member list has been removed
    /// </summary>
    /// <returns> Instance of the chat without members </returns>
    public ChatModel IgnoreMembers()
    {
        var copy = Clone();
        copy.Members = [];
        return copy;
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
    public List<ChatMemberModel> Members { get; set; }

    /// <summary>
    /// List of messages
    /// </summary>
    public List<ChatMessageModel> Messages { get; set; }

    /// <summary>
    /// Clone instance
    /// </summary>
    /// <returns> Clone </returns>
    private ChatModel Clone() => (ChatModel)MemberwiseClone();
}

/// <summary>
/// DTO for ChatModel class with extra data removed
/// </summary>
/// <param name="Id"> Id </param>
/// <param name="CreatorId"> Creator id </param>
/// <param name="Name"> Name </param>
/// <param name="CreationDt"> Creation datetime </param>
/// <param name="IsDeleted"> Whether the chat is deleted </param>
/// <param name="IsDirect"> Whether the chat is direct </param>
/// <param name="Members"> List of the members </param>
/// <param name="Messages"> List of the messages </param>
public record ChatModelResult(
    Guid Id, Guid? CreatorId, string Name, DateTime CreationDt, bool IsDeleted, bool IsDirect, 
    List<ChatModelResultMember> Members, List<ChatModelResultMessage> Messages);

/// <summary>
/// Reduced version of ChatMemberModel class
/// </summary>
/// <param name="UserId"> Member id </param>
/// <param name="IsAdmin"> Whether the member is admin </param>
public record ChatModelResultMember(Guid UserId, bool IsAdmin);

/// <summary>
/// Reduced version of ChatMessageModel class
/// </summary>
/// <param name="Id"> Id </param>
/// <param name="Content"> Content </param>
/// <param name="SentBy"> ID of user who sent the message </param>
/// <param name="SendingDt"> Sending datetime </param>
/// <param name="IsRedacted"> Whether the message is redacted </param>
/// <param name="IsDeleted"> Whether the message is deleted </param>
public record ChatModelResultMessage(Guid Id, string Content, Guid SentBy, DateTime SendingDt, bool IsRedacted, bool IsDeleted);