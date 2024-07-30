namespace MSG.Messenger.Core;

/// <summary>
/// Chat member model
/// </summary>
public class ChatMemberModel
{
    /// <summary>
    /// Existing member constructor
    /// </summary>
    /// <param name="userId"> User id </param>
    /// <param name="chatId"> Chat id </param>
    /// <param name="isAdmin"> Whether the member is admin </param>
    public ChatMemberModel(Guid userId, Guid chatId, bool isAdmin = false)
    {
        UserId = userId;
        ChatId = chatId;
        IsAdmin = isAdmin;
    }

    /// <summary>
    /// Chat id
    /// </summary>
    public Guid ChatId { get; set; }

    /// <summary>
    /// Member id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Whether the member is admin
    /// </summary>
    public bool IsAdmin { get; set; }
}