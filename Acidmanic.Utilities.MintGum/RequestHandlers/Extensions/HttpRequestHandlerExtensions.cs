using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers.Extensions;

internal static class HttpRequestHandlerExtensions
{

    private class DefaultRequestDescriptor : IRequestDescriptor
    {
        public DefaultRequestDescriptor(string nameKebabCase, string nameTitleCase, string methodName, string uri)
        {
            NameKebabCase = nameKebabCase;
            NameTitleCase = nameTitleCase;
            MethodName = methodName;
            Uri = uri;
        }

        public string NameKebabCase { get; }
        public string NameTitleCase { get; }
        public string MethodName { get; }
        public string Uri { get; }
    }
    
    
    public static IRequestDescriptor GerOrCreateDescriptor(this IHttpRequestHandler handler )
    {
        var descriptor = handler.GetDescriptor();

        if (descriptor is { } d) return d;

        return Create(handler);
    }




    private static DefaultRequestDescriptor Create(IHttpRequestHandler handler)
    {
        var names = handler.GetType().ReCase(
            "requesthandler",
            ConventionDescriptor.Standard.Kebab,
            ExtraNameConventions.Title
        );

        return new DefaultRequestDescriptor(names[0],
            names[1],
            handler.Method.Method,
            handler.RoutePath);
    }
}