using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Base.Authentication.Integration;

/// <summary>
/// Methods for extending <see cref="IServiceCollection"/> to integrate authentication
/// </summary>
public static class AuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// Add jwt bearer authentication to the DI container
    /// </summary>
    /// <param name="services"> Service collection </param>
    /// <param name="securityKey"> Security key </param>
    /// <param name="jwtBearerEvents"> Events from <see cref="JwtBearerHandler"/> </param>
    /// <returns> Configured service collection </returns>
    public static IServiceCollection AddJwtBearerAuthentication(
        this IServiceCollection services,
        string securityKey,
        JwtBearerEvents? jwtBearerEvents = null)
    {
        if (string.IsNullOrWhiteSpace(securityKey))
            throw new ArgumentException("Security key not set", nameof(securityKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.RequireHttpsMetadata = false;
                opts.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                if (jwtBearerEvents != null)
                    opts.Events = jwtBearerEvents;
            });

        return services;
    }

    /// <summary>
    /// Add a Swagger configured to authenticate the web client
    /// </summary>
    /// <param name="services"> Service collection </param>
    /// <param name="xmlFilePath"> Path to the xml file with documentation </param>
    /// <returns> Configured service collection </returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFilePath)
    {
        services.AddSwaggerGen(opts =>
        {
            opts.IncludeXmlComments(xmlFilePath, true);

            opts.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = @"Enter access token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        },
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
