using Acidmanic.Utilities.MintGum.Extensions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class IntroduceRequestHandler : RequestHandlerBase
{
    public override string RoutePath => "";


    protected override Task PerformHandling()
    {
        var apis = new List<object>();

        var mintGum = Inject<MintGum>();

        foreach (var handler in RequestHandlersList.RequestHandlers)
        {
            var uri = mintGum.Configuration.MaintenanceApisBaseUri.JoinPath(handler.RoutePath);

            var url = $"{HttpContext.Request.Scheme}://".JoinPath(
                HttpContext.Request.Host.Value,
                mintGum.Configuration.MaintenanceApisBaseUri,
                handler.RoutePath);

            apis.Add(new
            {
                Name = handler.Name,
                Uri = uri,
                Url = url,
                Method = handler.Method.Method,
            });
        }

        return Ok(apis);
    }
}