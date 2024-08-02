using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class RestoreZippedFilesRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Post;

    public override string Description =>
        "Will clear the ContentRoot directory, and extract the contents of all the uploaded zipped files into it. " +
        "you can send upload files into this endpoint using Multipart form fields." ;


    public RestoreZippedFilesRequestHandler()
    {
        BuildScheme( b => b.AddMultipart(MultipartValueType.File,"any-name","path/to/zip-file.zip"));
    }
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