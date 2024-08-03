using Acidmanic.Utilities.MintGum.Extensions;
using Acidmanic.Utilities.MintGum.Insomnia.Models;
using Acidmanic.Utilities.MintGum.RequestHandling;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class InsomniaRequestHandler : RequestHandlerBase
{

    public override string Description =>
        "Provides a json describing the Mint-Gum Apis." +
        " It can be used to be imported into Insomnia Rest Client";
    
    protected override Task PerformHandling()
    {
        var document = new InsomniaDocument("Mint-gum-export");

        var mintGum = Inject<MintGum>();

        var folderId = document.AddFolder("Mint-gum").Id;
        
        var mintBaseEnvVar = new EnvironmentKeyValuePair("mint_uri", mintGum.Configuration.MaintenanceApisBaseUri);
        
        document.BaseEnvironment.Add(mintBaseEnvVar);
        
        var currentEnvironment = document.AddEnvironment(HttpContext.Request.Host.Host);

        var baseUrlEnvVar = new EnvironmentKeyValuePair("base_url", BaseUrl);
        
        currentEnvironment.Add(baseUrlEnvVar);

        var descriptors = RequestHandlersList.RequestDescriptors;
        
        foreach (var descriptor in descriptors)
        {
            
            var requestUrl = $"{baseUrlEnvVar}/{mintBaseEnvVar}/{descriptor.Uri}";

            requestUrl = requestUrl.Replace("//", "/");
            
            var request = document.AddRequest(descriptor.NameKebabCase,
                descriptor.Description, 
                descriptor.MethodName, 
                requestUrl, folderId);

            request.Body = descriptor.Translate(HttpContext.RequestServices);

        }

        return Ok(document);
    }
    
}