using System.Reflection;
using Base.Authentication.Core;
using Base.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Base.DataAccess;

/// <summary>
/// Data base context
/// </summary>
/// <param name="options"></param>
public class DataBaseContext(DbContextOptions<DataBaseContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }

    internal DbSet<Feature> Features { get; set; }

    internal DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Role>().HasData(
            new Role()
            {
                Name = "DefaultRole",
                Description = "Default role"
            });
    }
}