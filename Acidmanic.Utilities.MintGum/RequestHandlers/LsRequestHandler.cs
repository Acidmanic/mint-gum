using Acidmanic.Utilities.MintGum.RequestHandling;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class LsRequestHandler : RequestHandlerBase
{
    private record LsRequest(string? SearchPattern);

    public override string Description => "Returns a list of all files (recursively) inside ContentRoot directory. " +
                                          "Given a search pattern, it will only return matching results.";


    public LsRequestHandler()
    {
        BuildScheme(b => b.SetJsonBody(typeof(LsRequest)));
    }
    
    protected override async Task PerformHandling()
    {
        var request = await ReadRequestBody<LsRequest>();

        var cr = Inject<ContentRootService>();

        var content = cr.ListAllContent(request?.SearchPattern);

        await Ok(new { Content = content });
    }
}