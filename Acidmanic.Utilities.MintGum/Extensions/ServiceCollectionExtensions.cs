using Acidmanic.Utilities.MintGum.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace Acidmanic.Utilities.MintGum.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMintGum(this IServiceCollection services)
        => AddMintGum(services, cb => { });

    public static void AddMintGum(this IServiceCollection services, Action<IMintGumConfigurationBuilder> configure)
    {
        var configurationBuilder = new MintGumConfigurationBuilder();

        configure(configurationBuilder);

        services.AddSingleton<StaticServerConfigurator>(sp =>
        {
            var logger = sp.GetService<ILogger>() ?? NullLogger.Instance;

            return new StaticServerConfigurator(configurationBuilder, logger);
        });
    }
}