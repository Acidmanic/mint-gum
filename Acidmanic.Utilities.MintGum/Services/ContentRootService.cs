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

    public void Delete(string pattern)
    {
        var directories = Directory.GetDirectories(_mintGum.ServingDirectoryPath,
            pattern, SearchOption.AllDirectories);

        foreach (var directory in directories)
        {
            try
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory,true);
                }
            }
            catch 
            {
                /* Ignore */
            }
        }

        var files = Directory.GetFiles(_mintGum.ServingDirectoryPath, pattern);

        foreach (var file in files)
        {
            try
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch 
            {
                /* Ignore */
            }
        }
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

    public void Rename(string sourceName, string destinationName)
    {

        var sourcePath = Path.Join(_mintGum.ServingDirectoryPath, sourceName);
        
        var destinationPath = Path.Join(_mintGum.ServingDirectoryPath, destinationName);
        
        Directory.Move(sourcePath,destinationPath);
        
    }
}