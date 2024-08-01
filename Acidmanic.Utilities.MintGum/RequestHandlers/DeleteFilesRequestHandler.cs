using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class DeleteFilesRequestHandler : RequestHandlerBase
{
    private record DeleteFilesRequest(string SearchPattern);
    
    protected override async Task PerformHandling()
    {
        var request = await ReadRequestBody<DeleteFilesRequest>();

        var pattern = "*";

        if (request is { } r && !string.IsNullOrEmpty(r.SearchPattern))
        {
            pattern = r.SearchPattern;
        }
        
        var cr = Inject<ContentRootService>();

        cr.Delete(pattern);

        var filesList = cr.ListAllContent();
        
        await Ok(filesList);
    }
}