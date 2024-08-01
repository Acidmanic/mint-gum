using System.IO.Compression;

namespace Acidmanic.Utilities.MintGum.Services;

internal class ContentRootService
{
    private readonly MintGum _mintGum;


    public string ContentRootDirectoryPath => _mintGum.ServingDirectoryPath;
    
    public ContentRootService(MintGum mintGum)
    {
        _mintGum = mintGum;
    }


    public void ClearContent(string? defaultFileContent = null)
    {
        Directory.Delete(_mintGum.ServingDirectoryPath, true);

        Directory.CreateDirectory(_mintGum.ServingDirectoryPath);

        _mintGum.CreateDefaultIndexFile(defaultFileContent);
    }


    public List<string> ListAllContent(string? pattern = null)
    {
        var directory = new DirectoryInfo(_mintGum.ServingDirectoryPath);

        var searchPattern = pattern ?? string.Empty;

        var result = directory.GetFiles(searchPattern, SearchOption.AllDirectories);

        var baseLower = _mintGum.ServingDirectoryPath.ToLower();

        Func<string, string> unBase = p =>
        {
            if (p.ToLower().StartsWith(baseLower))
            {
                p = p.Substring(baseLower.Length, p.Length - baseLower.Length);
            }

            return p;
        };
        return result.Select(f => f.FullName).Select(unBase).ToList();
    }

    public List<string> RestoreZip(byte[] data,bool clearContent = false)
    {
        if (clearContent)
        {
            ClearContent();
        }

        var zip = new ZipArchive(new MemoryStream(data));

        zip.ExtractToDirectory(_mintGum.ServingDirectoryPath, true);

        return ListAllContent();
    }
}