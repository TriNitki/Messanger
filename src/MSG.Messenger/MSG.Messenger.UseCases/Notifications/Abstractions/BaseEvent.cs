using MediatR;

namespace MSG.Messenger.UseCases.Notifications.Abstractions;

/// <summary>
/// Base interface for event
/// </summary>
public interface IBaseEvent : INotification
{
    /// <summary>
    /// Whether the event was successful
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Collection of errors
    /// </summary>
    public IReadOnlyCollection<string>? Errors { get; set; }

    /// <summary>
    /// Caller connection ids
    /// </summary>
    public string CallerConnectionId { get; set; }
}