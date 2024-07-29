using System.IO.Compression;
using System.Net;

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

    public async Task<List<string>> RestoreZip(byte[] data)
    {
        Directory.Delete(_mintGum.ServingDirectoryPath, true);
        var zip = new ZipArchive(new MemoryStream(data));

        foreach (var entry in zip.Entries)
        {
            var filePath = Path.Join(_mintGum.ServingDirectoryPath, entry.FullName);

            await ExtractInto(entry.Open(), entry.Length, filePath);
        }

        return ListAllContent();
    }


    private async Task ExtractInto(Stream stream, long length, string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        long read = 0;

        var buffer = new byte[10240];

        var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        while (read < length)
        {
            var r = await stream.ReadAsync(buffer, 0, buffer.Length);

            await fileStream.WriteAsync(buffer, 0, r);

            read += r;
        }
    }
}