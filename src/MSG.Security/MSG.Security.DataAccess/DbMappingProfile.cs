using AutoMapper;
using MSG.Security.Authentication.Core;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess;

/// <summary>
/// Data base mapper configuration
/// </summary>
public class DbMappingProfile : Profile
{
    public DbMappingProfile()
    {
        CreateMap<User, AuthUser>()
            .ConstructUsing(x => new AuthUser(
                x.Id,
                x.Login,
                x.HashedPassword,
                x.IsBlocked,
                x.Email,
                Array.Empty<string>()))
            .AfterMap((x, y) => y.Roles = x.Roles.Select(role => role.RoleId).ToArray());

        CreateMap<AuthUser, User>()
            .ForMember(x => x.Roles, x => x.Ignore())
            .AfterMap((x, y) => y.Roles = x.Roles.Select(role => new RoleToUser()
            {
                RoleId = role,
                UserId = x.Id
            }).ToList());

        CreateMap<Client, AuthClient>()
            .ConstructUsing(x => new AuthClient(
                x.Name,
                x.HashedSecret));

        CreateMap<AuthClient, Client>();
    }
}