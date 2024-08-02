using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers.Extensions;

internal static class HttpRequestHandlerExtensions
{

    private class DefaultRequestDescriptor : IRequestDescriptor
    {
        public DefaultRequestDescriptor(string nameKebabCase, string nameTitleCase, string methodName, string uri, string description)
        {
            NameKebabCase = nameKebabCase;
            NameTitleCase = nameTitleCase;
            MethodName = methodName;
            Uri = uri;
            Description = description;
        }

        public string NameKebabCase { get; }
        public string NameTitleCase { get; }
        public string MethodName { get; }
        public string Uri { get; }
        
        public string Description { get; }
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

        string description = names[1] + " endpoint.";
        
        return new DefaultRequestDescriptor(names[0],
            names[1],
            handler.Method.Method,
            handler.RoutePath,
            description);
    }
}