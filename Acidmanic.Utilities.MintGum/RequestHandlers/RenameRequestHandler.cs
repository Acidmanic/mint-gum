using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class RenameRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Put;

    public override string Description => "Renames a file or directory.";

    private record RenameRequest(string SourcePath, string NewName);


    public RenameRequestHandler()
    {
        BuildScheme(b => b.SetJsonBody(typeof(RenameRequest)));
    }
    
    protected override async Task PerformHandling()
    {
        var request = await ReadRequestBody<RenameRequest>();
        
        if (request is { } r 
            && !string.IsNullOrEmpty(r.SourcePath)
            && !string.IsNullOrEmpty(r.NewName))
        {
            var contentRoot = Inject<ContentRootService>();

            contentRoot.Rename(request.SourcePath, request.NewName);
            
            var filesList = contentRoot.ListAllContent();
        
            await Ok(filesList);
        }
        
        await BadRequest(new { Message = "Invalid Source/Destination Name" });
    }
}