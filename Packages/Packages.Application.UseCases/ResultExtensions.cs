using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Packages.Application.UseCases;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<TResponseValue>(this in Result<TResponseValue> result)
    {
        ResultStatus status = result.Status;

        IActionResult result2 = status switch
        {
            ResultStatus.Ok => new OkObjectResult(result.GetValue()),
            ResultStatus.Created => new ObjectResult(result.GetValue())
            {
                StatusCode = (int)result.Status
            },
            ResultStatus.NoContent => new NoContentResult(),
            ResultStatus.Invalid => new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.Status
            },
            ResultStatus.Forbidden => new ForbidResult(),
            ResultStatus.Conflict => new ConflictObjectResult(result.Errors),
            ResultStatus.Error => new ObjectResult(result.Errors)
            {
                StatusCode = (int)result.Status
            },
            _ => throw new NotSupportedException(),
        };

        return result2;
    }
}
