using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Winton.Extensions.Configuration.Consul;

namespace Packages.Application.Consul;

internal static class HostConfigurationExtension
{
    internal static IConfigurationBuilder AddConsulBeforeEnv(
        this IConfigurationBuilder builder,
        string consulUri,
        params string[] cfgNodes)
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

        builder.AddConsul(consulUri, index != -1 ? index : Index.End, cfgNodes);
        return builder;
    }

    private static void AddConsul(this IConfigurationBuilder builder,
        string consulUri,
        Index insertPosition,
        params string[] cfgNodes)
    {
        if (cfgNodes.Length == 0) return;

        if (!Uri.TryCreate(consulUri, UriKind.Absolute, out var result))
            throw new ConsulConfigurationException("Consul connection string (ConsulUri) is not specified or has an invalid format");

        if (insertPosition.Equals(Index.End))
        {
            AddNodes(builder, result, cfgNodes);
        }
        else
        {
            var configurationBuilder = new ConfigurationBuilder();
            AddNodes(configurationBuilder, result, cfgNodes);

            var configurationSource = new ChainedConfigurationSource()
            {
                Configuration = configurationBuilder.Build(),
                ShouldDisposeConfiguration = true
            };

            var offset = insertPosition.GetOffset(builder.Sources.Count);
            builder.Sources.Insert(offset, configurationSource);
        }
    }

    private static void AddNodes(
        IConfigurationBuilder configurationBuilder,
        Uri uri,
        string[] cfgNodes)
    {
        var options1 = (Action<IConsulConfigurationSource>)(options =>
        {
            options.ConsulConfigurationOptions = (Action<ConsulClientConfiguration>)(cco => cco.Address = uri);
            options.Optional = true;
        });

        if (options1 == null) throw new ArgumentNullException(nameof(options1));

        foreach (var cfgNode in cfgNodes)
            configurationBuilder.AddConsul(cfgNode, options1);
    }
}