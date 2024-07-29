using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class ExtRequestHandler : RequestHandlerBase
{
    protected override async Task PerformHandling()
    {
     
        var cr = Inject<ContentRootService>();

        var zipData = await File.ReadAllBytesAsync("e.zip");
        
        var content = cr.RestoreZip(zipData);

        await Ok(new { Content = content });
    }
}