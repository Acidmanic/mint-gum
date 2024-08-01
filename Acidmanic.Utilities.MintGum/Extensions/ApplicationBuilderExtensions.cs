using Acidmanic.Utilities.MintGum.RequestHandlers;
using Microsoft.Extensions.Logging.Abstractions;


namespace Acidmanic.Utilities.MintGum.Extensions;

public static class ApplicationBuilderExtensions
{
    private static MintGum GetMintGum(IApplicationBuilder app)
    {
        var mintGum = app.ApplicationServices.GetService<MintGum>();

        if (mintGum is null) throw new Exception("You Should Add MintGum to your project services.");

        return mintGum;
    }


    public static void ConfigureMintGumProvider(this IApplicationBuilder app)
    {
        var mintGum = GetMintGum(app);

        mintGum.ConfigurePreRouting(app);
    }


    public static void MapMintGum(this IApplicationBuilder app)
    {
        var logger = app.ApplicationServices.GetService<ILogger>() ?? NullLogger.Instance;
        
        var mintGum = GetMintGum(app);

        mintGum.ConfigureMappings(app);

        if (mintGum.Configuration.AddMaintenanceApis)
        {
            app.UseEndpoints(c =>
            {
                foreach (var handler in RequestHandlersList.RequestHandlers)
                {
                    
                    var pattern = JoinPath(mintGum.Configuration.MaintenanceApisBaseUri, handler.RoutePath);

                    IEndpointConventionBuilder? builder = null;
                    
                    switch (handler.Method.Method.ToLower())
                    {
                        case "get":
                            builder = c.MapGet(pattern, handler.Handle);
                            break;
                        case "post":
                            builder = c.MapPost(pattern, handler.Handle);
                            break;
                        case "put":
                            builder = c.MapPut(pattern, handler.Handle);
                            break;
                        case "delete":
                            builder = c.MapDelete(pattern, handler.Handle);
                            break;
                    }

                    if (builder is { } b)
                    {
                        if (mintGum.Configuration.MaintenanceApisRequireAuthorization)
                        {
                            b.RequireAuthorization();
                        }
                        
                        logger.LogInformation("Mint Gum Api: [{Method}] {Path}",handler.Method.Method, pattern);
                    }
                }
            });
        }
    }

    private static string JoinPath(params string[] segments)
    {
        var result = "";

        foreach (var segment in segments)
        {
            var delimmiter = "";
            
            if (!result.EndsWith("/") && !segment.StartsWith("/"))
            {
                delimmiter = "/";
            }

            result += delimmiter + segment;
        }

        return result;
    }
}