using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class RestoreFilesRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Post;


    protected override async Task PerformHandling()
    {
        var files = await ReadUploadedFiles();

        var contentRoot = Inject<ContentRootService>();
        
        contentRoot.ClearContent();
        
        foreach (var uploadedFile in files)
        {
            var filePath = Path.Join(contentRoot.ContentRootDirectoryPath,uploadedFile.FileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            await File.WriteAllBytesAsync(filePath,uploadedFile.FileData);
        }

        var filesList = contentRoot.ListAllContent();
        
        await Ok(filesList);
    }

    
}