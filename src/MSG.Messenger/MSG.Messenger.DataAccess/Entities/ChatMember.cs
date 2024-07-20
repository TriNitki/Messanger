namespace MSG.Messenger.DataAccess.Entities;

/// <summary>
/// Chat member
/// </summary>
public class ChatMember
{
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