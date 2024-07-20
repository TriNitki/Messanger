using MediatR;
using MSG.Messenger.Core;
using Packages.Application.UseCases;

namespace MSG.Messenger.UseCases.Commands.GetOrCreateDirectChat;

public class GetOrCreateDirectChatCommand : IRequest<Result<ChatModel>>
{
    public Guid Sender { get; set; }

    public Guid Receiver { get; set; }

    public GetOrCreateDirectChatCommand(Guid sender, Guid receiver)
    {
        Sender = sender;
        Receiver = receiver;
    }
}