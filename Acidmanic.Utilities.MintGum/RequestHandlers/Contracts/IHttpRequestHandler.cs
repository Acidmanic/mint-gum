namespace Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

internal interface IHttpRequestHandler
{
    HttpMethod Method { get; }
    
    string Path { get; }

    Task Handle(HttpContext context);
}