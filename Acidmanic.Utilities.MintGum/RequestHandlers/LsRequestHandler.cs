using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class LsRequestHandler : RequestHandlerBase
{
    private record LsRequest(string? Pattern);

    protected override async Task PerformHandling()
    {
        var request = await ReadRequestBody<LsRequest>();

        var cr = Inject<ContentRootService>();

        var content = cr.ListAllContent(request?.Pattern);

        await Ok(new { Content = content });
    }
}