using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSG.Messenger.Contracts;
using MSG.Messenger.UseCases.Commands.DeleteMessage;
using MSG.Messenger.UseCases.Commands.RedactMessage;
using MSG.Messenger.UseCases.Commands.SendMessage;
using MSG.Security.Authorization;
using Packages.Application.UseCases;

namespace MSG.Messenger.Service.Controllers;

/// <summary>
/// Provides RestAPI to work with messages
/// </summary>
[Route("api/chats/")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserAccessor _userAccessor;

    /// <summary>
    /// Controller constructor
    /// </summary>
    /// <param name="mediator"> Mediator </param>
    /// <param name="userAccessor"> User accessor </param>
    public MessageController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
    }

    /// <summary>
    /// Send message
    /// </summary>
    /// <param name="chatId"> Chat id </param>
    /// <param name="request"> Request </param>
    /// <response code="201"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPost("{chatId:guid})")]
    public async Task<IActionResult> Send(Guid chatId, SendMessageRequest request)
    {
        var result = await _mediator.Send(new SendMessageCommand(chatId, _userAccessor.Id, request.Message));
        return result.ToActionResult();
    }

    /// <summary>
    /// Delete message
    /// </summary>
    /// <param name="messageId"> Message id </param>
    /// <response code="204"> Success </response>
    /// <response code="400"> Incorrect data passed </response>
    [HttpDelete("messages/{messageId:guid}")]
    public async Task<IActionResult> Delete(Guid messageId) 
    {
        var result = await _mediator.Send(new DeleteMessageCommand(messageId, _userAccessor.Id));
        return result.ToActionResult();
    }

    /// <summary>
    /// Redact message
    /// </summary>
    /// <param name="messageId"> Message id </param>
    /// <param name="request"> Request </param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPatch("messages/{messageId:guid}")]
    public async Task<IActionResult> Redact(Guid messageId, RedactMessageRequest request)
    {
        var result = await _mediator.Send(new RedactMessageCommand(messageId, _userAccessor.Id, request.NewContent));
        return result.ToActionResult();
    }
}