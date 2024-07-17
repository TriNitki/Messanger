using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Packages.Application.Data.DI;

/// <summary>
/// Extension to <see cref="IServiceCollection"/> that adds data access configuration.
/// </summary>
public static class ServiceCollectionDataAccessExtension
{
    /// <summary>
    /// Db provider
    /// </summary>
    private const string DbProvider = "Npgsql";

    /// <summary>
    /// Add database context
    /// </summary>
    /// <typeparam name="TContext"> Context </typeparam>
    /// <param name="services"> Service Collection </param>
    /// <param name="cfg"> Application Configuration </param>
    /// <returns> Service Collection </returns>
    public static IServiceCollection AddDataContext<TContext>(this IServiceCollection services, IConfiguration cfg)
        where TContext : DbContext
    {
        string connectionString = cfg.GetConnectionString(DbProvider) 
                                  ?? throw new ArgumentNullException(null, "Db connection string not specified");
        
        return services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseSnakeCaseNamingConvention();
        });
    }
}
