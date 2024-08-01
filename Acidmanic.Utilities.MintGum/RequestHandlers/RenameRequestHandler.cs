using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class RenameRequestHandler : RequestHandlerBase
{
    private record RenameRequest(string Name, string NewName);
    
    protected override async Task PerformHandling()
    {
        var request = await ReadRequestBody<RenameRequest>();
        
        if (request is { } r 
            && !string.IsNullOrEmpty(r.Name)
            && !string.IsNullOrEmpty(r.NewName))
        {
            var contentRoot = Inject<ContentRootService>();

            contentRoot.Rename(request.Name, request.NewName);
            
            var filesList = contentRoot.ListAllContent();
        
            await Ok(filesList);
        }
        
        await BadRequest(new { Message = "Invalid Source/Destination Name" });
    }
}