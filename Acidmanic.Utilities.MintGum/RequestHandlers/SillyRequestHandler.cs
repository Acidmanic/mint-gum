using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class SillyRequestHandler:RequestHandlerBase
{
    protected override Task PerformHandling()
    {
        return Ok(new {Message="Lalalalala"});
    }
}