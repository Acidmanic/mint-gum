using Acidmanic.Utilities.MintGum.RequestHandling;
using Acidmanic.Utilities.MintGum.RequestHandling.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class DeleteFilesRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Delete;

    public override string Description =>
        "Deletes every file and directory matching the given pattern inside the ContentRoot directory.";
    
    private record DeleteFilesRequest([SuggestedValue("/styles/*.old.css")]string SearchPattern);


    public DeleteFilesRequestHandler()
    {
        BuildScheme(b => b.SetJsonBody(typeof(DeleteFilesRequest)));
    }
    
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