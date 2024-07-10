using Base.Authentication.UseCases.Commands.RegisterService;
using Base.Authorization.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Packages.Application.UseCases;

namespace Base.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServiceController(IMediator mediator)
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
        [Permission("RegisterNewService")]
        public async Task<IActionResult> Register(RegisterServiceCommand request)
        {
            var result = await _mediator.Send(request);
            return result.ToActionResult();
        }
    }
}
