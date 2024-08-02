namespace Packages.Application.UseCases;

/// <summary>
/// Value wrapper containing status code
/// </summary>
/// <typeparam name="TResponseValue"> Wrapped value </typeparam>
public readonly struct Result<TResponseValue>
{
    private const string StatusErrorMsg = "Status {0} does not imply a result";

    private const string ValueErrorMsg = "Result value is not specified";

    private readonly TResponseValue? _value = default;

    public ResultStatus Status { get; } = ResultStatus.Unknown;

    public bool IsSuccess => Status.IsSuccess();

    public IReadOnlyCollection<string>? Errors { get; } = null;

    #region factories
    /// <summary>
    /// Mark result as status code 200 (OK)
    /// </summary>
    /// <param name="value"> Value </param>
    /// <returns> Result with status code and value </returns>
    public static Result<TResponseValue> Success(TResponseValue value) => new(value);
    /// <summary>
    /// Mark result as status code 201 (Created)
    /// </summary>
    /// <param name="value"> Value </param>
    /// <returns> Result with status code and value </returns>
    public static Result<TResponseValue> Created(TResponseValue value) => new(value, ResultStatus.Created);
    /// <summary>
    /// Mark result as status code 204 (No Content)
    /// </summary>
    /// <returns> Result with status code </returns>
    public static Result<TResponseValue> NoContent() => new(ResultStatus.NoContent);
    /// <summary>
    /// Mark result as status code 400 (Bad Request)
    /// </summary>
    /// <param name="errors"> Errors </param>
    /// <returns> Result with status code and errors </returns>
    public static Result<TResponseValue> Invalid(IReadOnlyCollection<string> errors) => new(errors, ResultStatus.Invalid);
    /// <summary>
    /// Mark result as status code 400 (Bad Request)
    /// </summary>
    /// <param name="error"> Error </param>
    /// <returns> Result with status code and error </returns>
    public static Result<TResponseValue> Invalid(string error) => new(new List<string> { error }, ResultStatus.Invalid);
    /// <summary>
    /// Mark result as status code 401 (Unauthorized)
    /// </summary>
    /// <param name="errors"> Errors </param>
    /// <returns> Result with status code and errors </returns>
    public static Result<TResponseValue> Unauthorized(IReadOnlyCollection<string> errors) => new(errors, ResultStatus.Unauthorized);

    /// <summary>
    /// Mark result as status code 401 (Unauthorized)
    /// </summary>
    /// <param name="error"> Error </param>
    /// <returns> Result with status code and error </returns>
    public static Result<TResponseValue> Unauthorized(string error) => new(new List<string> { error }, ResultStatus.Unauthorized);
    /// <summary>
    /// Mark result as status code 403 (Forbidden)
    /// </summary>
    /// <returns> Result with status code </returns>
    public static Result<TResponseValue> Forbidden() => new(ResultStatus.Forbidden);
    /// <summary>
    /// Mark result as status code 409 (Conflict)
    /// </summary>
    /// <param name="errors"> Errors </param>
    /// <returns> Result with status code and errors </returns>
    public static Result<TResponseValue> Conflict(IReadOnlyCollection<string> errors) => new(errors, ResultStatus.Conflict);
    /// <summary>
    /// Mark result as status code 409 (Conflict)
    /// </summary>
    /// <param name="error"> Error </param>
    /// <returns> Result with status code and error </returns>
    public static Result<TResponseValue> Conflict(string error) => new(new List<string> { error }, ResultStatus.Conflict);
    /// <summary>
    /// Mark result as status code 422 (Unprocessable Entity)
    /// </summary>
    /// <param name="errors"> List of errors </param>
    /// <returns> Result with status code and list of errors </returns>
    public static Result<TResponseValue> Error(IReadOnlyCollection<string> errors) => new(errors, ResultStatus.Error);
    /// <summary>
    /// Mark result as status code 422 (Unprocessable Entity)
    /// </summary>
    /// <param name="error"> Error </param>
    /// <returns> Result with status code and error </returns>
    public static Result<TResponseValue> Error(string error) => new(new List<string> { error }, ResultStatus.Error);
    #endregion

    private Result(TResponseValue value, ResultStatus status = ResultStatus.Ok)
    {
        _value = value;
        Status = status;
    }

    private Result(IReadOnlyCollection<string> errors, ResultStatus status)
    {
        Status = status;
        Errors = errors;
    }

    private Result(ResultStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Get the value
    /// </summary>
    /// <returns>Value</returns>
    /// <exception cref="AppException">
    /// The result status does not imply a result
    /// Either the value is missing
    /// </exception>
    public TResponseValue GetValue()
    {
        if (!Status.IsSuccess() || _value == null)
            throw new AppException((!Status.IsSuccess()) ? string.Format(StatusErrorMsg, Status) : ValueErrorMsg);

        return _value;
    }

    /// <summary>
    /// Tries to get the value or returns the default value
    /// </summary>
    /// <returns> Value or default value </returns>
    public TResponseValue? GetValueOrDefault()
    {
        return _value;
    }
}