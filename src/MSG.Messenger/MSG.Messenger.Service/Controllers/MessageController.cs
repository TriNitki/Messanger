using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSG.Messenger.Contracts;
using MSG.Messenger.UseCases.Commands.DeleteMessage;
using MSG.Messenger.UseCases.Commands.RedactMessage;
using MSG.Messenger.UseCases.Commands.SendMessage;
using MSG.Security.Authorization;
using Packages.Application.UseCases;

namespace MSG.Messenger.Service.Controllers;

[Route("api/chats/")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserAccessor _userAccessor;

    public MessageController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
    }

    [HttpPost("{chatId:guid})")]
    public async Task<IActionResult> Send(Guid chatId, SendMessageRequest request)
    {
        var result = await _mediator.Send(new SendMessageCommand(chatId, _userAccessor.Id, request.Message));
        return result.ToActionResult();
    }

    [HttpDelete("messages/{messageId:guid}")]
    public async Task<IActionResult> Delete(Guid messageId) 
    {
        var result = await _mediator.Send(new DeleteMessageCommand(messageId, _userAccessor.Id));
        return result.ToActionResult();
    }

    [HttpPatch("messages/{messageId:guid}")]
    public async Task<IActionResult> Redact(Guid messageId, RedactMessageRequest request)
    {
        var result = await _mediator.Send(new RedactMessageCommand(messageId, _userAccessor.Id, request.NewContent));
        return result.ToActionResult();
    }
}