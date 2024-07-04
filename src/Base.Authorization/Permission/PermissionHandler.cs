using Base.Permission.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Base.Authorization.Permission;

/// <summary>
/// Permission based authorization handler
/// </summary>
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ILogger<PermissionHandler> _logger;
    private readonly IUserAccessor _userAccessor;
    private readonly IFeatureAccessProvider _featureAccessProvider;

    public PermissionHandler(
        ILogger<PermissionHandler> logger,
        IUserAccessor userAccessor,
        IFeatureAccessProvider featureAccessProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userAccessor = userAccessor ?? throw new ArgumentNullException(nameof(userAccessor));
        _featureAccessProvider = featureAccessProvider ?? throw new ArgumentNullException(nameof(featureAccessProvider));
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!context.User?.Identity!.IsAuthenticated ?? true)
        {
            context.Fail();
            return;
        }

        if (await _featureAccessProvider.HasAccess(requirement.Feature, _userAccessor.Roles))
        {
            context.Succeed(requirement);
        }
        else
        {
            var reason = $"User {_userAccessor.Login} don't have permission to {requirement.Feature} feature";
            _logger.LogInformation(reason);
            context.Fail(new AuthorizationFailureReason(this, reason));
        }
    }
}