using Base.Authentication.Contracts;
using Base.Authentication.Core;
using Base.Authentication.Core.Options;
using Base.Authentication.UseCases.Abstractions;
using Base.Authentication.UseCases.Commands.Login;
using MediatR;
using Microsoft.Extensions.Options;
using Packages.Application.UseCases;

namespace Base.Authentication.UseCases.Commands.Registration;

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
