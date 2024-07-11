using MediatR;
using Microsoft.Extensions.Options;
using MSG.Security.Authentication.Contracts;
using MSG.Security.Authentication.Core;
using MSG.Security.Authentication.Core.Options;
using MSG.Security.Authentication.UseCases.Abstractions;
using MSG.Security.Authentication.UseCases.Commands.Login;
using Packages.Application.UseCases;

namespace MSG.Security.Authentication.UseCases.Commands.Registration;

public class RegistrationCommandHandler : LoginBaseHandler, IRequestHandler<RegistrationCommand, Result<Tokens>>
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordOptions _passwordOptions;
    private readonly string[] _defaultUserRoles;

    public RegistrationCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        IOptions<PasswordOptions> passwordOptions,
        IOptions<RoleOptions> roles) : base(tokenService)
    {
        _userRepository = userRepository;
        _passwordOptions = passwordOptions.Value;
        _defaultUserRoles = roles.Value.DefaultUserRoles.ToArray() ?? throw new ArgumentNullException(nameof(roles));
    }

    public async Task<Result<Tokens>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ResolveAsync(request.Login) is not null)
            return Result<Tokens>.Conflict("User with this login already exists");

        var user = await _userRepository.CreateAsync(
                new AuthUser(request.Login, request.Password, _passwordOptions, _defaultUserRoles));

        return await Handle(user);
    }
}
