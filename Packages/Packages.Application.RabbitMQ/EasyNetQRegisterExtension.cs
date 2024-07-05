using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Packages.Application.RabbitMQ;

public static class EasyNetQRegisterExtension
{
    /// <summary>
    /// Add EasyNetQ to DI
    /// </summary>
    /// <param name="services"> DI Services </param>
    /// <param name="configuration"> Application Configuration </param>
    /// <param name="assemblies"> Assemblies </param>
    /// <returns> DI Services Collection </returns>
    public static IServiceCollection AddEasyNetQ(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly[] assemblies)
    {
        string? connectionString = configuration["RabbitMQConnection"];
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(null, "Connection string to RabbitMQ is not specified");

        string? subscriptionIdPrefix = configuration["ServiceName"];
        if (string.IsNullOrEmpty(subscriptionIdPrefix))
            throw new ArgumentNullException(null, "Service name not specified");

        services.RegisterEasyNetQ(connectionString, register => register.EnableMicrosoftLogging());
        
        List<Type> consumers = new();
        foreach (Type consumer in GetConsumers(assemblies))
        {
            services.AddTransient(consumer);
            consumers.Add(consumer);
        }

        services.AddSingleton(provider => new AutoSubscriberWrapper(
            provider.GetRequiredService<IServiceResolver>(), 
            consumers.ToArray(), 
            subscriptionIdPrefix));
        
        services.AddHostedService<BusSubscriber<AutoSubscriberWrapper>>();
        return services;
    }

    /// <summary>
    /// Add message consumers to the DI
    /// </summary>
    /// <param name="assemblies"> Assemblies </param>
    private static IEnumerable<Type> GetConsumers(Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        foreach (var type in types)
        {
            if (type is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false } &&
                type.GetInterfaces().Any(x => x.IsGenericType && 
                                              x.GetGenericTypeDefinition() is var definition && 
                                              (definition == typeof(IConsumeAsync<>) || definition == typeof(IConsume<>))))
            {
                yield return type;
            }
        }
    }
}