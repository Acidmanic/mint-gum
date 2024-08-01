using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

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
        new IntroduceRequestHandler()
    };
}