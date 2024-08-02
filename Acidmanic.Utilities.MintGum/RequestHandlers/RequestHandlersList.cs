using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.RequestHandlers.Extensions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal static class RequestHandlersList
{
    public static readonly List<IHttpRequestHandler> RequestHandlers = new()
    {
        new ClearRequestHandler(),
        new LsRequestHandler(),
        new DeleteFilesRequestHandler(),
        new RestoreZippedFilesRequestHandler(),
        new RestoreFilesRequestHandler(),
        new RenameRequestHandler(),
        new IntroduceRequestHandler(),
        new InsomniaRequestHandler()
    };

    public static List<IRequestDescriptor> RequestDescriptors => RequestHandlers
        .Select(r => r.GerOrCreateDescriptor())
        .ToList();
}