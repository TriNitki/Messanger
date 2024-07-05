using Microsoft.AspNetCore.Mvc;

namespace Packages.Application.UseCases;

/// <summary>
/// Extensions for <see cref="Result{T}"/>
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Create <see cref="IActionResult"/> based on <see cref="Result{T}"/>
    /// </summary>
    /// <param name="result">Result of command execution</param>
    /// <exception cref="NotSupportedException">Not supported result</exception>
    public static IActionResult ToActionResult<TResponseValue>(this in Result<TResponseValue> result)
    {
        return result.Status switch
        {
            ResultStatus.Ok => new OkObjectResult(result.GetValue()),
            ResultStatus.Created => new ObjectResult(result.GetValue()) { StatusCode = (int)result.Status },
            ResultStatus.NoContent => new NoContentResult(),
            ResultStatus.Invalid => new ObjectResult(result.Errors) { StatusCode = (int)result.Status },
            ResultStatus.Forbidden => new ForbidResult(),
            ResultStatus.Conflict => new ConflictObjectResult(result.Errors),
            ResultStatus.Error => new ObjectResult(result.Errors) { StatusCode = (int)result.Status },
            ResultStatus.Unauthorized => new ObjectResult(result.Errors) { StatusCode = (int)result.Status},
            _ => throw new NotSupportedException()
        };
    }
}
