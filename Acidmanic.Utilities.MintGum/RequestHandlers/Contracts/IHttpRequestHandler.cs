namespace Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

internal interface IHttpRequestHandler
{
    HttpMethod Method { get; }
    
    string RoutePath { get; }
    
    string Name { get; }

    Task Handle(HttpContext context);
}