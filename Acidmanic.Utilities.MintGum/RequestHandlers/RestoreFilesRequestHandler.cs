using Acidmanic.Utilities.MintGum.RequestHandling;
using Acidmanic.Utilities.MintGum.RequestHandling.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class RestoreFilesRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Post;


    public override string Description =>
        "Will clear the ContentRoot directory, and write all the uploaded files into it. " +
        "you can send upload files into this endpoint using Multipart form fields." ;


    public RestoreFilesRequestHandler()
    {
        BuildScheme( b => b.AddMultipart(MultipartValueType.File,"any-name","path/to/file.html"));
    }
    
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