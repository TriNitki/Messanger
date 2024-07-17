namespace MSG.Security.Authentication.Contracts;

/// <summary>
/// Client login request model
/// </summary>
/// <param name="Name"> ServiceName </param>
/// <param name="Secret"> ServiceSecret </param>
public record LoginClientRequest(string Name, string Secret);