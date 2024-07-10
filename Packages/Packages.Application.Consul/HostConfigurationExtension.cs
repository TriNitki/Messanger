using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Winton.Extensions.Configuration.Consul;

namespace Packages.Application.Consul;

/// <summary>
/// Extensions for application configuration
/// </summary>
internal static class HostConfigurationExtension
{
    /// <summary>
    /// Obtain configurations from Consul K/V.
    /// Configuration obtained from environment variables and from startup arguments have a higher priority.
    /// </summary>
    /// <param name="builder">Configuration Builder.</param>
    /// <param name="consulUri">Address Consul.</param>
    /// <param name="cfgNodes">Node names with configurations in Consul K/V.</param>
    internal static IConfigurationBuilder AddConsulBeforeEnv(this IConfigurationBuilder builder, string consulUri, params string[] cfgNodes)
    {
        var sources = builder.Sources;
        int index;

        for (index = sources.Count - 1; index >= 0; --index)
        {
            bool flag;
            if (sources[index] is EnvironmentVariablesConfigurationSource configurationSource)
            {
                var prefix = configurationSource.Prefix;
                if (string.IsNullOrEmpty(prefix))
                {
                    flag = true;
                    goto label_5;
                }
            }

            flag = false;
            label_5:
            if (flag)
                break;
        }

        return builder.AddConsul(consulUri, index != -1 ? index : Index.End, cfgNodes);
    }

    /// <summary>
    /// Get configurations from Consul K/V
    /// </summary>
    /// <param name="builder">Configuration Builder</param>
    /// <param name="consulUri">Consul's address</param>
    /// <param name="insertPosition">Position of the new configuration source relative to the already added ones</param>
    /// <param name="cfgNodes">Node names with configurations in Consul K/V</param>
    private static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder,
        string consulUri,
        Index insertPosition,
        params string[] cfgNodes)
    {
        if (cfgNodes.Length == 0) 
            return builder;

        if (!Uri.TryCreate(consulUri, UriKind.Absolute, out var uri))
            throw new ConsulConfigurationException("Consul connection string (ConsulUri) is not specified or has an invalid format");

        if (insertPosition.Equals(Index.End))
        {
            AddNodes(builder, uri, cfgNodes);
        }
        else
        {
            var tempBuilder = new ConfigurationBuilder();
            AddNodes(tempBuilder, uri, cfgNodes);
            var source = new ChainedConfigurationSource { Configuration = tempBuilder.Build(), ShouldDisposeConfiguration = true };

            int insertAt = insertPosition.GetOffset(builder.Sources.Count);
            builder.Sources.Insert(insertAt, source);
        }

        return builder;
    }

    private static void AddNodes(IConfigurationBuilder configurationBuilder, Uri uri, string[] cfgNodes)
    {
        var action = (IConsulConfigurationSource options) =>
        {
            options.ConsulConfigurationOptions = cco => cco.Address = uri;
            options.Optional = true;
        };

        foreach (var cfgNode in cfgNodes)
            configurationBuilder.AddConsul(cfgNode, action);
    }
}