using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Packages.Application.Data.DI;

public static class ServiceCollectionDataAccessExtension
{
    private const string DbProvider = "Npgsql";

    public static IServiceCollection AddDataContext<TContext>(
        this IServiceCollection services,
        IConfiguration cfg)
        where TContext : DbContext
    {
        string connectionString = cfg.GetConnectionString(DbProvider) 
                                  ?? throw new ArgumentNullException(null, "Не задана строка подключения к БД");
        
        return services.AddDbContext<TContext>(options =>
        {
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Base.Service"));
            options.UseSnakeCaseNamingConvention();
        });
    }
}
