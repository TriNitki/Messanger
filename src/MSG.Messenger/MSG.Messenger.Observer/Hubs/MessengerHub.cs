using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MSG.Messenger.Observer.Hubs.Clients;
using MSG.Messenger.UseCases.Commands.AddMember;
using MSG.Messenger.UseCases.Commands.CreateGroupChat;
using MSG.Messenger.UseCases.Commands.DeleteMessage;
using MSG.Messenger.UseCases.Commands.EditAdmin;
using MSG.Messenger.UseCases.Commands.GetOrCreateDirectChat;
using MSG.Messenger.UseCases.Commands.KickMember;
using MSG.Messenger.UseCases.Commands.LeaveGroupChat;
using MSG.Messenger.UseCases.Commands.RedactMessage;
using MSG.Messenger.UseCases.Commands.RenameChat;
using MSG.Messenger.UseCases.Commands.SendMessage;
using MSG.Messenger.UseCases.Notifications;
using MSG.Security.Authorization;

namespace MSG.Messenger.Observer.Hubs;

/// <summary>
/// Hub for messenger interaction
/// </summary>
[Authorize]
public class MessengerHub : Hub<IMessengerClient>
{
    private readonly UserAccessor _userAccessor;

    private readonly IMediator _mediator;

    private static readonly ConcurrentDictionary<Guid, HashSet<string>> ConnectedUsers = [];

    public MessengerHub(UserAccessor userAccessor, IMediator mediator)
    {
        _userAccessor = userAccessor;
        _mediator = mediator;
    }
        
    public override Task OnConnectedAsync()
    {
        if (!ConnectedUsers.TryAdd(_userAccessor.Id, [Context.ConnectionId]))
            ConnectedUsers[_userAccessor.Id].Add(Context.ConnectionId);

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (!ConnectedUsers.TryGetValue(_userAccessor.Id, out var userConnections))
            return Task.CompletedTask;

        if (userConnections.Count <= 1)
            ConnectedUsers.TryRemove(_userAccessor.Id, out _);
        else
            ConnectedUsers[_userAccessor.Id].Remove(Context.ConnectionId);

        return Task.CompletedTask;
    }

    #region Chats
    public async Task CreateGroupChat(string name, HashSet<Guid> members)
    {
        members.Add(_userAccessor.Id);
        var connections = GetUserConnections(members);

        var result = await _mediator.Send(new CreateGroupChatCommand(name, members, _userAccessor.Id));
        await _mediator.Publish(new CreateChatEvent(result, Context.ConnectionId, connections));
    }

    public async Task CreateDirectChat(Guid receiverId)
    {
        var connections = GetUserConnections([_userAccessor.Id, receiverId]);

        var result = await _mediator.Send(new GetOrCreateDirectChatCommand(_userAccessor.Id, receiverId));
        await _mediator.Publish(new CreateChatEvent(result, Context.ConnectionId, connections));
    }

    public async Task LeaveGroupChat(Guid chatId)
    {
        var result = await _mediator.Send(new LeaveGroupChatCommand(_userAccessor.Id, chatId));
        var leaveMemberConnections = GetUserConnections(_userAccessor.Id);

        await _mediator.Publish(new LeaveGroupChatEvent(
            result, Context.ConnectionId, _userAccessor.Id, leaveMemberConnections));
    }

    public async Task KickGroupChatMember(Guid chatId, Guid memberId)
    {
        var result = await _mediator.Send(new KickMemberCommand(chatId, _userAccessor.Id, memberId));
        var kickingMemberConnections = GetUserConnections(memberId);

        await _mediator.Publish(
            new KickGroupChatMemberEvent(result, _userAccessor.Id, memberId, kickingMemberConnections, Context.ConnectionId));
    }

    public async Task AddGroupChatMember(Guid chatId, Guid memberId)
    {
        var result = await _mediator.Send(new AddMemberCommand(chatId, _userAccessor.Id, memberId));
        var addingMemberConnections = GetUserConnections(memberId);

        await _mediator.Publish(new AddGroupChatMemberEvent(result, Context.ConnectionId, memberId, addingMemberConnections));
    }

    public async Task RenameGroupChat(Guid chatId, string newName)
    {
        var result = await _mediator.Send(new RenameChatCommand(chatId, _userAccessor.Id, newName));

        await _mediator.Publish(new RenameGroupChatEvent(result, Context.ConnectionId));
    }

    public async Task EditGroupChatAdmin(Guid chatId, Guid memberId, bool isAdmin)
    {
        var result = await _mediator.Send(new EditAdminCommand(chatId, _userAccessor.Id, memberId, isAdmin));

        await _mediator.Publish(new EditGroupChatAdminEvent(result, Context.ConnectionId, isAdmin, memberId));
    }
    #endregion

    #region Messages
    public async Task SendMessage(Guid chatId, string message)
    {
        var result = await _mediator.Send(new SendMessageCommand(chatId, _userAccessor.Id, message));
        await _mediator.Publish(new SendMessageEvent(result, Context.ConnectionId));
    }

    public async Task DeleteMessage(Guid messageId)
    {
        var result = await _mediator.Send(new DeleteMessageCommand(messageId, _userAccessor.Id));
        await _mediator.Publish(new DeleteMessageEvent(result, Context.ConnectionId));
    }

    public async Task RedactMessage(Guid messageId, string newMessage)
    {
        var result = await _mediator.Send(new RedactMessageCommand(messageId, _userAccessor.Id, newMessage));
        await _mediator.Publish(new RedactMessageEvent(result, Context.ConnectionId));
    }
    #endregion

    private static HashSet<string> GetUserConnections(Guid user)
    {
        ConnectedUsers.TryGetValue(user, out var userConnections);
        return userConnections ?? [];
    }

    private static HashSet<string> GetUserConnections(IEnumerable<Guid> users)
    {
        HashSet<string> connections = [];
        foreach (var user in users)
        {
            ConnectedUsers.TryGetValue(user, out var userConnections);
            if (userConnections != null)
                connections.UnionWith(userConnections);
        }
        return connections;
    }
}