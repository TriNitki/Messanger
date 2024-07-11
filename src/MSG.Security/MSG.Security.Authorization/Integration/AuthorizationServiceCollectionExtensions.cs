using Dodo.HttpClientResiliencePolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSG.Security.Authentication.Clients;
using MSG.Security.Authentication.Clients.Abstractions;
using MSG.Security.Authentication.Contracts;
using MSG.Security.Authorization.Permission;
using MSG.Security.Permission.Clients;
using Refit;

namespace MSG.Security.Authorization.Integration;

/// <summary>
/// Methods for extending<see cref="IServiceCollection"/> to integrate authentication
/// </summary>
public static class AuthorizationServiceCollectionExtensions
{
    public static IServiceCollection AddPermissionBasedAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authorizationServiceName = configuration.GetAuthorizationServiceName();

        // refit setup
        services.AddRefitClient<IPermissionClient>()
            .ConfigureHttpClient(x => x.BaseAddress = configuration.GetServiceUri(authorizationServiceName))
            .AddHttpMessageHandler<AuthHeaderHandler>()
            .AddResiliencePolicies();

        services.AddRefitClient<IAuthorizationClient>()
            .ConfigureHttpClient(x => x.BaseAddress = configuration.GetServiceUri(authorizationServiceName))
            .AddResiliencePolicies();


        // Caching setup
        services.AddMemoryCache();


        // Permission based authorization setup
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
        services.AddTransient<IAuthorizationHandler, PermissionHandler>();
        services.AddTransient<IFeatureAccessProvider, FeatureAccessProvider>();


        // Jwt setup
        services.Configure<JwtTokenOptions>(x =>
        {
            x.AccessTokenExpirationInMinutes
                = configuration.GetValue(nameof(JwtTokenOptions.AccessTokenExpirationInMinutes), 15);
        });


        // Authorized user setup
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddHttpContextAccessor();

        return services;
    }

    /// <summary>
    /// Get Uri address of the service
    /// </summary>
    /// <param name="configuration"> Configuration </param>
    /// <param name="serviceName"> Service name </param>
    /// <returns> Uri address of the service </returns>
    /// <exception cref="ArgumentException"> <see cref="AuthEnvironmentVariables.FabioUrl"/> is not specified </exception>
    private static Uri GetServiceUri(this IConfiguration configuration, string serviceName)
    {
        if (!Uri.TryCreate(configuration[AuthEnvironmentVariables.FabioUrl], UriKind.Absolute, out var fabioUrl))
            throw new ArgumentException("Fabio Url is not specified");

        return new Uri(fabioUrl, serviceName);
    }

    /// <summary>
    /// Get authorization service name
    /// </summary>
    /// <param name="configuration"> Configuration </param>
    /// <returns> Authorization service name </returns>
    /// <exception cref="ArgumentException"> <see cref="AuthEnvironmentVariables.AuthorizationServiceName"/> is not specified </exception>
    private static string GetAuthorizationServiceName(this IConfiguration configuration)
    {
        var authorizationServiceName = configuration[AuthEnvironmentVariables.AuthorizationServiceName];

        if (string.IsNullOrEmpty(authorizationServiceName))
            throw new ArgumentException("Authorization service name is not specified");

        return authorizationServiceName;
    }
}