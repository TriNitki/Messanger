using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSG.Security.Authentication.Clients;
using MSG.Security.Authentication.Clients.Abstractions;
using MSG.Security.Authentication.Clients.Providers;
using MSG.Security.Authentication.Contracts;
using MSG.Security.Authorization.Client;
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
        var authorizationServiceUri = configuration.GetAuthorizationServiceUri();

        if (string.IsNullOrEmpty(configuration[nameof(ClientOptions.ServiceName)]))
            throw new ArgumentException("Client's name is not specified");

        if (string.IsNullOrEmpty(configuration[nameof(ClientOptions.ServiceSecret)]))
            throw new ArgumentException("Client's secret is not specified");


        services.AddTransient<AuthHeaderHandler>();
        services.AddSingleton<IClientTokenProvider, ClientTokenProvider>();

        // refit setup
        services.AddRefitClient<IPermissionClient>()
            .ConfigureHttpClient(x => x.BaseAddress = configuration.GetServiceUri(authorizationServiceUri))
            .AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddRefitClient<IAuthorizationClient>()
            .ConfigureHttpClient(x => x.BaseAddress = configuration.GetServiceUri(authorizationServiceUri));


        // Caching setup
        services.AddMemoryCache();


        // Policy provider
        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        // Permission based authorization setup
        services.AddTransient<IAuthorizationHandler, PermissionHandler>();
        services.AddTransient<IFeatureAccessProvider, FeatureAccessProvider>();

        // Client credential flow authorization
        services.AddTransient<IAuthorizationHandler, ClientHandler>();

        services.Configure<ClientOptions>(x =>
        {
            x.ServiceName = configuration[nameof(ClientOptions.ServiceName)]!;
            x.ServiceSecret = configuration[nameof(ClientOptions.ServiceSecret)]!;
        });

        // Jwt setup
        services.Configure<JwtTokenOptions>(x =>
        {
            x.AccessTokenExpirationInMinutes
                = configuration.GetValue(nameof(JwtTokenOptions.AccessTokenExpirationInMinutes), 15);
        });


        // Authorized client setup
        services.AddScoped<IUserAccessor, UserAccessor>();
        services.AddScoped<IClientAccessor, ClientAccessor>();
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
    /// <exception cref="ArgumentException"> <see cref="AuthEnvironmentVariables.SecurityServiceUri"/> is not specified </exception>
    private static string GetAuthorizationServiceUri(this IConfiguration configuration)
    {
        var authorizationServiceUri = configuration[AuthEnvironmentVariables.SecurityServiceUri];

        if (string.IsNullOrEmpty(authorizationServiceUri))
            throw new ArgumentException("Authorization service Uri is not specified");

        return authorizationServiceUri;
    }
}