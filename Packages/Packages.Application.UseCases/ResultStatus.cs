namespace Application.UseCases;

public enum ResultStatus
{
    Unknown = 0,
    Ok = 200,
    Created = 201,
    NoContent = 204,
    Invalid = 400,
    Unauthorized = 401,
    Forbidden = 403,
    Conflict = 409,
    Error = 422
}
