using Acidmanic.Utilities.MintGum.Services;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal class ClearRequestHandler : RequestHandlerBase
{
    private record ClearRequest(string? Content);

    public override HttpMethod Method => HttpMethod.Delete;

    public override string Description => "Deletes everything in the ContentRoot directory. " +
                                          "And creates a simple default page. " +
                                          "If Content is provided in the request, it will be written into default page file.";

    protected override async Task PerformHandling()
    {
        var request = await ReadRequestBody<ClearRequest>();

        var cr = Inject<ContentRootService>();

        cr.ClearContent(request?.Content);

        await Ok();
    }
}