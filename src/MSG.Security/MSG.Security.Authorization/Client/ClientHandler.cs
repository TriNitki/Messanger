using Microsoft.AspNetCore.Authorization;

namespace MSG.Security.Authorization.Client;

/// <summary>
/// Client credential flow authorization handler
/// </summary>
public class ClientHandler : AuthorizationHandler<ClientRequirement>
{
    private readonly IClientAccessor _clientAccessor;

    public ClientHandler(IClientAccessor clientAccessor)
    {
        _clientAccessor = clientAccessor ?? throw new ArgumentNullException(nameof(clientAccessor));
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientRequirement requirement)
    {
        if (!context.User?.Identity!.IsAuthenticated ?? true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var clientId = _clientAccessor.Id;

        if (string.IsNullOrEmpty(clientId))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}