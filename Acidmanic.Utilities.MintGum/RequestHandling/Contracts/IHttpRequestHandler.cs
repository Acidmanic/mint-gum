namespace Acidmanic.Utilities.MintGum.RequestHandling.Contracts;

internal interface IHttpRequestHandler
{
    HttpMethod Method { get; }
    
    string RoutePath { get; }

    Task Handle(HttpContext context);

    IRequestDescriptor? GetDescriptor();
}