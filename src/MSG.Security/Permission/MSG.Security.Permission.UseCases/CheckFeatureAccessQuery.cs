using MediatR;
using Packages.Application.UseCases;

namespace MSG.Security.Permission.UseCases;

public class CheckFeatureAccessQuery : IRequest<Result<bool>>
{
    /// <summary>
    /// Feature
    /// </summary>
    public string Feature { get; }

    /// <summary>
    /// Array of roles
    /// </summary>
    public string[] Roles { get; }

    public CheckFeatureAccessQuery(string feature, string[] roles)
    {
        Feature = feature;
        Roles = roles;
    }
}