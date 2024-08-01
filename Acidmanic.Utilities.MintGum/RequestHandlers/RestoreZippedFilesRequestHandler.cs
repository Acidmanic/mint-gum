using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class RestoreZippedFilesRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Post;


    protected override async Task PerformHandling()
    {
        var files = await ReadUploadedFiles();

        var contentRoot = Inject<ContentRootService>();
        
        contentRoot.ClearContent();
        
        foreach (var uploadedFile in files)
        {
            contentRoot.RestoreZip(uploadedFile.FileData, false);
        }
        
        var filesList = contentRoot.ListAllContent();
        
        await Ok(filesList);
    }

    
}