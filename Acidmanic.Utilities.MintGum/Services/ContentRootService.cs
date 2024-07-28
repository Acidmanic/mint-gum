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
        Directory.Delete(_mintGum.ServingDirectoryPath, true);

        Directory.CreateDirectory(_mintGum.ServingDirectoryPath);

        _mintGum.CreateDefaultIndexFile(defaultFileContent);
    }


    public List<string> ListAllContent(string? pattern)
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
}