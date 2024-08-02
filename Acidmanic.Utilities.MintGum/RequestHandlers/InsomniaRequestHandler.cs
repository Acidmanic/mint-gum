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

        var descriptors = RequestHandlersList.RequestDescriptors;
        
        foreach (var descriptor in descriptors)
        {
            var uri = mintGum.Configuration.MaintenanceApisBaseUri.JoinPath(descriptor.Uri);

            var url = $"{HttpContext.Request.Scheme}://".JoinPath(
                HttpContext.Request.Host.Value,
                mintGum.Configuration.MaintenanceApisBaseUri,
                descriptor.Uri);

            document.AddRequest(descriptor.NameKebabCase,
                descriptor.NameTitleCase, 
                descriptor.MethodName, 
                uri, folderId);

        }

        return Ok(document);
    }
}