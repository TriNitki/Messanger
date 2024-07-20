using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MSG.Security.Authentication.Core;
using MSG.Security.DataAccess.Entities;

namespace MSG.Security.DataAccess;

/// <summary>
/// Data base context
/// </summary>
/// <param name="options"> Options to be used by DbContext </param>
public class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }

    internal DbSet<Feature> Features { get; set; }

    internal DbSet<RefreshToken> RefreshTokens { get; set; }

    internal DbSet<RefreshTokenFamily> RefreshTokenFamilies { get; set; }

    internal DbSet<Client> Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Name = "DefaultRole",
                Description = "Default role"
            }, 
            new Role
            {
                Name = "Service",
                Description = "Service"
            },
            new Role
            {
                Name = "Admin",
                Description = "Admin"
            });

        modelBuilder.Entity<Feature>().HasData(
            new Feature
            {
                Name = "FeatureIsAvailable",
                Description = "Check if the feature is accessible for the passed roles"
            },
            new Feature
            {
                Name = "RegisterNewClient",
                Description = "Register new service"
            });

        modelBuilder.Entity<RoleToFeature>().HasData(
            new RoleToFeature
            {
                RoleId = "Service",
                FeatureId = "FeatureIsAvailable",
            },
            new RoleToFeature
            {
                RoleId = "Admin",
                FeatureId = "RegisterNewClient"
            });

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = Guid.Parse("3b39ec8f-70e9-4220-99e3-c4fad04ba574"),
                Login = "string",
                HashedPassword = "D1884DFD78131E1088052B42F7D2632FA0FAC9D225187B559F8D1CF6B10FB8AD",
                IsBlocked = false
            });

        modelBuilder.Entity<RoleToUser>().HasData(
            new RoleToUser
            {
                RoleId = "Admin",
                UserId = Guid.Parse("3b39ec8f-70e9-4220-99e3-c4fad04ba574")
            });

        modelBuilder.Entity<Client>().HasData(
            new Client
            {
                Name = "messenger",
                HashedSecret = "2194A9AE27296881A182060A2FDCE05DA88AE0E8BAD6B7B7526504744256452F"
            });
    }
}