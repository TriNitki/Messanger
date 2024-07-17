using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSG.Security.Authentication.Contracts;
using MSG.Security.Authentication.UseCases.Commands.ChangePassword;
using MSG.Security.Authentication.UseCases.Commands.InvalidateRefreshToken;
using MSG.Security.Authentication.UseCases.Commands.Login;
using MSG.Security.Authentication.UseCases.Commands.LoginClient;
using MSG.Security.Authentication.UseCases.Commands.RefreshTokens;
using MSG.Security.Authentication.UseCases.Commands.Registration;
using Packages.Application.UseCases;

namespace MSG.Security.Service.Controllers;

/// <summary>
/// Provides RestAPI to work with user authorisation
/// </summary>
[Route("api/auth")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// User registration
    /// </summary>
    /// <param name="request"> User data </param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    /// <response code="409"> User with the passed username already exists </response>
    [HttpPost("registration")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    [ProducesResponseType(typeof(List<string>), 409)]
    public async Task<IActionResult> Registration(RegistrationCommand request)
    {
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }

    /// <summary>
    /// User authorization
    /// </summary>
    /// <param name="request"> User data </param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPost("login/user")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var result = await _mediator.Send(new LoginCommand(request.Login, request.Password));
        return result.ToActionResult();
    }

    /// <summary>
    /// Client authorization
    /// </summary>
    /// <param name="request"> Client data </param>
    /// <response code="204"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPost("login/client")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> LoginClient(LoginClientRequest request)
    {
        var result = await _mediator.Send(
            new LoginClientCommand(request.Name, request.Secret));
        return result.ToActionResult();
    }

    /// <summary>
    /// Change user's password
    /// </summary>
    /// <param name="request"> Change password request </param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPut("changePassword")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var result = await _mediator.Send(
            new ChangePasswordCommand(request.Login, request.Password, request.NewPassword));
        return result.ToActionResult();
    }

    /// <summary>
    /// Get a new token pair by refresh token
    /// </summary>
    /// <param name="request"></param>
    /// <response code="200"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPost("refreshTokens")]
    [ProducesResponseType(typeof(Tokens), 200)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> RefreshTokens(RefreshTokenRequest request)
    {
        var result = await _mediator.Send(new RefreshTokensCommand(request.RefreshToken));
        return result.ToActionResult();
    }

    /// <summary>
    /// Deactivate refresh token
    /// </summary>
    /// <param name="request"> Invalidated token request </param>
    /// <response code="204"> Success </response>
    /// <response code="400"> Passed object was not validated </response>
    [HttpPut("invalidateRefreshToken")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(List<string>), 400)]
    public async Task<IActionResult> InvalidateRefreshToken(InvalidateRefreshTokenCommand request)
    {
        var result = await _mediator.Send(request);
        return result.ToActionResult();
    }
}