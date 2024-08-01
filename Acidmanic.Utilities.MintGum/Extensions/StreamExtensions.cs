namespace Acidmanic.Utilities.MintGum.Extensions;

internal  static class StreamExtensions
{
    
    
    
    public static async Task<byte[]> ReadAsBytesArrayAsync(this Stream stream, long length)
    {
        var buffer = new byte[10240];

        var output = new MemoryStream();

        var read = 0L;

        while (read < length)
        {
            var r = await stream.ReadAsync(buffer, 0, buffer.Length);

            await output.WriteAsync(buffer, 0, r);

            read += r;
        }

        return output.GetBuffer();
    }

    public static async Task<byte[]> ReadAsBytesArrayAsync(this Stream stream)
    {
        var buffer = new byte[10240];

        var output = new MemoryStream();

        var read = 0L;

        var r = 1;

        while (r > 0)
        {
            r = await stream.ReadAsync(buffer, 0, buffer.Length);

            await output.WriteAsync(buffer, 0, r);
            
            if (r > 0) read += r;
        }

        return output.GetBuffer();
    }
}