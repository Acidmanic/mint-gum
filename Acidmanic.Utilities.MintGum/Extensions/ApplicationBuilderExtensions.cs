namespace Acidmanic.Utilities.MintGum.Extensions;

public static class ApplicationBuilderExtensions
{
    private static StaticServerConfigurator GetMintGum(IApplicationBuilder app)
    {
        var mintGum = app.ApplicationServices.GetService<StaticServerConfigurator>();

        if (mintGum is null) throw new Exception("You Should Add MintGum to your project services.");

        return mintGum;
    }


    public static void ConfigureMintGumProvider(this IApplicationBuilder app, IHostEnvironment env)
    {
        var mintGum = GetMintGum(app);

        mintGum.ConfigurePreRouting(app, env);
    }


    public static void MapMintGum(this IApplicationBuilder app, IHostEnvironment env)
    {
        var mintGum = GetMintGum(app);

        mintGum.ConfigureMappings(app, env);
    }
}