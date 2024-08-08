using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSG.Messenger.UseCases.Queries.GetChat;
using MSG.Messenger.UseCases.Queries.GetChats;
using MSG.Security.Authorization;
using Packages.Application.UseCases;

namespace MSG.Messenger.Service.Controllers;

/// <summary>
/// Provides RestAPI to work with chats
/// </summary>
[Route("api/chats")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserAccessor _userAccessor;

    /// <summary>
    /// Controller constructor
    /// </summary>
    /// <param name="mediator"> Mediator </param>
    /// <param name="userAccessor"> User accessor </param>
    public ChatController(IMediator mediator, IUserAccessor userAccessor)
    {
        _mediator = mediator;
        _userAccessor = userAccessor;
    }

    /// <summary>
    /// Get chat info and messages
    /// </summary>
    /// <param name="chatId"> Chat id </param>
    /// <param name="fromMessage"> Start message index </param>
    /// <param name="toMessage"> End message index </param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Incorrect data passed </response>
    [HttpGet("{chatId:guid}")]
    public async Task<IActionResult> GetChat(Guid chatId, [FromQuery] uint fromMessage = 0, [FromQuery] uint toMessage = 25)
    {
        var result = await _mediator.Send(new GetChatQuery(chatId, _userAccessor.Id, (int)fromMessage, (int)toMessage));
        return result.ToActionResult();
    }

    /// <summary>
    /// Get user active chats
    /// </summary>
    /// <param name="fromChat"> Start chat index </param>
    /// <param name="toChat"> End chat index </param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Incorrect data passed </response>
    [HttpGet]
    public async Task<IActionResult> GetChats([FromQuery] uint fromChat = 0, [FromQuery] uint toChat = 25)
    {
        var result = await _mediator.Send(new GetChatsQuery(_userAccessor.Id, (int)fromChat, (int)toChat));
        return result.ToActionResult();
    }
}