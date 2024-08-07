using MediatR;

namespace MSG.Messenger.UseCases.Notifications.Abstractions;

public interface IBaseEvent : INotification
{
    public bool IsSuccess { get; set; }

    public IReadOnlyCollection<string>? Errors { get; set; }

    public string CallerConnectionId { get; set; }
}