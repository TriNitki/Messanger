using Base.Authentication.Contracts;
using Base.Authentication.Core.Options;
using Base.Authentication.Core.Services;
using Base.Authentication.UseCases.Abstractions;
using MediatR;
using Microsoft.Extensions.Options;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.Login;

public class LoginCommandHandler : LoginBaseHandler, IRequestHandler<LoginCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;
    private readonly string _salt;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IOptions<PasswordOptions> passwordOptions) : base(tokenService)
    {
        _userRepository = userRepository;
        _salt = passwordOptions.Value.Salt ?? throw new ArgumentNullException(nameof(passwordOptions));
    }

    public async Task<Result<Tokens>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.ResolveAsync(
            request.Login,
            CryptographyService.HashPassword(request.Password, _salt));

        if (user is null)
            return Result<Tokens>.Invalid("Invalid password or login");

        if (user.IsBlocked)
            return Result<Tokens>.Unauthorized("User is blocked");

        return await Handle(user);
    }
}
