using Acidmanic.Utilities.MintGum.Extensions;
using Acidmanic.Utilities.MintGum.RequestHandlers.Extensions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class IntroduceRequestHandler : RequestHandlerBase
{
    public override string RoutePath => "";

    public override string Description => "Describes Mint-Gum api in json format. ";

    protected override Task PerformHandling()
    {
        var apis = new List<object>();

        var mintGum = Inject<MintGum>();

        foreach (var handler in RequestHandlersList.RequestHandlers)
        {
            var descriptor = handler.GerOrCreateDescriptor();
            
            var uri = mintGum.Configuration.MaintenanceApisBaseUri.JoinPath(handler.RoutePath);

            var url = $"{HttpContext.Request.Scheme}://".JoinPath(
                HttpContext.Request.Host.Value,
                mintGum.Configuration.MaintenanceApisBaseUri,
                handler.RoutePath);

            apis.Add(new
            {
                Name = descriptor.NameTitleCase,
                Uri = uri,
                Url = url,
                Method = handler.Method.Method,
                Description = descriptor.Description
            });
        }

        return Ok(apis);
    }
}