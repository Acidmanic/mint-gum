using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal static class RequestHandlersList
{

    public static readonly List<IHttpRequestHandler> RequestHandlers = new()
    {
        new ClearRequestHandler(),
        new LsRequestHandler(),
        new ExtRequestHandler(),
        new RestoreZippedFilesRequestHandler(),
        new RestoreFilesRequestHandler()
    };
}