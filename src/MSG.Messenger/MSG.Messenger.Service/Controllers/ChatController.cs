using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSG.Messenger.Contracts;
using MSG.Messenger.UseCases.Commands.CreateGroupChat;
using MSG.Messenger.UseCases.Commands.GetOrCreateDirectChat;
using MSG.Security.Authorization;
using Packages.Application.UseCases;

namespace MSG.Messenger.Service.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public ChatController(IMediator mediator, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        [HttpPost("createGroup")]
        public async Task<IActionResult> CreateGroup(CreateGroupChatRequest request)
        {
            request.Members.Add(_userAccessor.Id);
            var result = await _mediator.Send(new CreateGroupChatCommand(request.Name, request.Members, _userAccessor.Id));
            return result.ToActionResult();
        }

        [HttpPost("createDirect")]
        public async Task<IActionResult> CreateDirect(CreateDirectChatRequest request)
        {
            var result = await _mediator.Send(new GetOrCreateDirectChatCommand(_userAccessor.Id, request.ReceiverId));
            return result.ToActionResult();
        }

        [HttpDelete("{chatId:guid}/leaveGroup")]
        public async Task<IActionResult> LeaveGroup(Guid chatId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{chatId:guid}/kickMember")]
        public async Task<IActionResult> KickMember(Guid chatId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{chatId:guid}/addMember")]
        public async Task<IActionResult> AddMember(Guid chatId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{chatId:guid}/rename")]
        public async Task<IActionResult> Rename(Guid chatId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{chatId:guid}/editAdmin/{memberId:guid}")]
        public async Task<IActionResult> EditAdmin(Guid chatId, Guid memberId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{chatId:guid}")]
        public async Task<IActionResult> GetChat(Guid chatId)
        {
            throw new NotImplementedException();
        }
    }
}
