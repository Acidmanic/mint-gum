using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class ClearRequestHandler:RequestHandlerBase
{
    private record ClearRequest(string? Content);

    public override HttpMethod Method => HttpMethod.Delete;

    protected override async  Task PerformHandling()
    {
        var request = await ReadRequestBody<ClearRequest>();

        var cr = Inject<ContentRootService>();
        
        cr.ClearContent(request?.Content);
        
        await Ok();
    }
}