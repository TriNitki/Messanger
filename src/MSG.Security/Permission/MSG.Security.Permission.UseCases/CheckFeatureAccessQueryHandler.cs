using MediatR;
using MSG.Security.Permission.UseCases.Abstractions;
using Packages.Application.UseCases;

namespace MSG.Security.Permission.UseCases;

/// <summary>
/// Feature access check handler
/// </summary>
public class CheckFeatureAccessQueryHandler : IRequestHandler<CheckFeatureAccessQuery, Result<bool>>
{
    private readonly IPermissionRepository _permissionRepository;

    public CheckFeatureAccessQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
    }

    public async Task<Result<bool>> Handle(CheckFeatureAccessQuery request, CancellationToken cancellationToken)
    {
        var feature = await _permissionRepository.GetFeature(request.Feature);

        return feature is null 
            ? Result<bool>.Invalid("Invalid feature was passed") 
            : Result<bool>.Success(feature.IsAvailable(request.Roles));
    }
}