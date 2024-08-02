using Acidmanic.Utilities.DataTypes;
using Acidmanic.Utilities.MintGum.Extensions;
using Acidmanic.Utilities.MintGum.Insomnia.Models;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class InsomniaRequestHandler : RequestHandlerBase
{
    protected override Task PerformHandling()
    {
        var document = new InsomniaDocument("Mint-gum-export");

        var mintGum = Inject<MintGum>();

        var folderId = document.AddFolder("Mint-gum").Id;

        foreach (var handler in RequestHandlersList.RequestHandlers)
        {
            var uri = mintGum.Configuration.MaintenanceApisBaseUri.JoinPath(handler.RoutePath);

            var url = $"{HttpContext.Request.Scheme}://".JoinPath(
                HttpContext.Request.Host.Value,
                mintGum.Configuration.MaintenanceApisBaseUri,
                handler.RoutePath);

            document.AddRequest(handler.RoutePath,
                handler.Name, 
                handler.Method.Method, 
                uri, folderId);

        }

        return Ok(document);
    }
}