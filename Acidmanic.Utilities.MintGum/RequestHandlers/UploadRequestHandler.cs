using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class UploadRequestHandler : RequestHandlerBase
{
    public override HttpMethod Method => HttpMethod.Post;
    

    protected override async Task PerformHandling()
    {

        var info = new List<object>();
        
        // foreach (var formFile in HttpContext.Request.Form.Files)
        // {
        //     info.Add(new
        //     {
        //         Name=formFile.Name,
        //         Length=formFile.Length,
        //         Data=await Read(formFile.OpenReadStream(),formFile.Length)
        //     });
        // }
        var data = await Read(HttpContext.Request.Body, HttpContext.Request.ContentLength ?? 0);
        
        info.Add(new
        {
            Headers=HttpContext.Request.Headers,
            Content=data
        });

        if(File.Exists("f.jpg")) File.Delete("f.jpg");
        
        await File.WriteAllBytesAsync("f.jpg", data);
        
        await Ok(info);
    }
    
    private async Task<byte[]> Read(Stream stream, long length)
    {

        var buffer = new byte[10240];

        var output = new MemoryStream();

        var read = 0L;

        while (read<length)
        {
            var r = await stream.ReadAsync(buffer, 0, buffer.Length);

            await output.WriteAsync(buffer, 0, r);

            read += r;
        }

        return output.GetBuffer();
    }
}