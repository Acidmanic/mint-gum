using System.IO.Compression;

namespace Acidmanic.Utilities.MintGum.Services;

internal class ContentRootService
{
    private readonly MintGum _mintGum;

    private enum InclusionStrategy
    {
        Inclusive,
        Exclusive
    }

    public string ContentRootDirectoryPath => _mintGum.ServingDirectoryPath;

    public ContentRootService(MintGum mintGum)
    {
        _mintGum = mintGum;
    }


    private string SanitizePattern(string? pattern, InclusionStrategy inclusion = InclusionStrategy.Exclusive)
    {
        var defaultPattern = inclusion == InclusionStrategy.Inclusive
            ? "*"
            : "0573A53EB80B4E1899218A4C88ACCBEC:2C9663A5-FE4F-4DB5-9CDD-E6EFC236DA79";


        if (string.IsNullOrEmpty(pattern)) return defaultPattern;

        if (Path.IsPathRooted(pattern))
        {
            return defaultPattern;
        }

        return pattern;
    }


    public void ClearContent(string? defaultFileContent = null)
    {
        Directory.Delete(_mintGum.ServingDirectoryPath, true);

        Directory.CreateDirectory(_mintGum.ServingDirectoryPath);

        _mintGum.CreateDefaultIndexFile(defaultFileContent);
    }

    public void Delete(string pattern)
    {
        pattern = SanitizePattern(pattern, InclusionStrategy.Exclusive);

        var directories = Directory.GetDirectories(_mintGum.ServingDirectoryPath,
            pattern, SearchOption.AllDirectories);

        foreach (var directory in directories)
        {
            try
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
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
        pattern = SanitizePattern(pattern, InclusionStrategy.Inclusive);

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

    public List<string> RestoreZip(byte[] data, bool clearContent = false)
    {
        if (clearContent)
        {
            ClearContent();
        }

        var zip = new ZipArchive(new MemoryStream(data));

        zip.ExtractToDirectory(_mintGum.ServingDirectoryPath, true);

        return ListAllContent();
    }

    public bool Rename(string sourceFilePath, string destinationName)
    {
        var sourcePath = Path.Join(_mintGum.ServingDirectoryPath, sourceFilePath);

        if (File.Exists(sourcePath))
        {
            var sourceFileLocation = new FileInfo(sourcePath).Directory?.FullName;

            if (sourceFileLocation is { } location)
            {
                var destinationPath = Path.Join(location, destinationName);

                Directory.Move(sourcePath, destinationPath);

                return true;
            }
        }

        return false;
    }
}