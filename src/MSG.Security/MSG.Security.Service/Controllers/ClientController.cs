using MediatR;
using Microsoft.AspNetCore.Mvc;
using MSG.Security.Authentication.UseCases.Commands.RegisterClient;
using MSG.Security.Authorization.Permission;
using Packages.Application.UseCases;

namespace MSG.Security.Service.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Register new service
        /// </summary>
        /// <param name="request"> Request </param>
        /// <response code="200"> Success </response>
        /// <response code="403"> Insufficient access rights to the resource </response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(List<string>), 403)]
        [Permission("RegisterNewClient")]
        public async Task<IActionResult> Register(RegisterClientCommand request)
        {
            var result = await _mediator.Send(request);
            return result.ToActionResult();
        }
    }
}
