using Acidmanic.Utilities.MintGum.Configuration;
using Acidmanic.Utilities.MintGum.Services;
using Microsoft.AspNetCore.Http.Features;
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

        services.AddSingleton<MintGum>(sp =>
        {
            var logger = sp.GetService<ILogger>() ?? NullLogger.Instance;

            return new MintGum(configurationBuilder, logger);
        });

        services.AddScoped<ContentRootService>();


        services.Configure<FormOptions>(x =>
        {
            x.MultipartHeadersLengthLimit = Int32.MaxValue;
            x.MultipartBoundaryLengthLimit = Int32.MaxValue;
            x.MultipartBodyLengthLimit = Int64.MaxValue;
            x.ValueLengthLimit = Int32.MaxValue;
            x.BufferBodyLengthLimit = Int64.MaxValue;
            x.MemoryBufferThreshold = Int32.MaxValue;

        });
    }
}