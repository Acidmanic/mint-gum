namespace Acidmanic.Utilities.MintGum.Services;

internal class ContentRootService
{


    private readonly MintGum _mintGum;


    public ContentRootService(MintGum mintGum)
    {
        _mintGum = mintGum;
    }


    public void ClearContent(string? defaultFileContent)
    {
        Directory.Delete(_mintGum.ServingDirectoryPath,true);

        Directory.CreateDirectory(_mintGum.ServingDirectoryPath);
        
        _mintGum.CreateDefaultIndexFile(defaultFileContent);
    }
    
    
}