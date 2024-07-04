namespace Application.UseCases;

public static class ResultStatusExtensions
{
    public static bool IsSuccess(this ResultStatus status)
    {
        return status >= ResultStatus.Ok && status < (ResultStatus)300;
    }
}
