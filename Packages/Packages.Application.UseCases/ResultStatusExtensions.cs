namespace Packages.Application.UseCases;

/// <summary>
/// Enum extension <see cref="ResultStatus"/>
/// </summary>
public static class ResultStatusExtensions
{
    /// <summary>
    /// Whether result is successful
    /// </summary>
    /// <param name="status">Статус</param>
    /// <returns><see langword="true"/> if successful, otherwise <see langword="false"/></returns>
    public static bool IsSuccess(this ResultStatus status)
        => (int)status is >= 200 and < 300;
}
