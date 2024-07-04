using AutoMapper;
using Base.Authentication.Core;
using Base.DataAccess.Entities;

namespace Base.DataAccess;

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
                x.PasswordHash,
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
    }
}