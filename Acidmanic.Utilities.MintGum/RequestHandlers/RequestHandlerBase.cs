using System.Net;
using Acidmanic.Utilities.MintGum.Extensions;
using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal abstract class RequestHandlerBase : IHttpRequestHandler
{
    public virtual HttpMethod Method => HttpMethod.Get;

    private readonly string _routePathByClass;
    public string Name { get; }

    protected record UploadedFile(string FileName, string FormField, byte[] FileData, long Length);


    public RequestHandlerBase()
    {
        var convertedName = GetPathByClass(
            ConventionDescriptor.Standard.Kebab,
            new ConventionDescriptor
            {
                Delimiter = " ",
                Name = "Title",
                Separation = Separation.ByDelimiter,
                PreFix = string.Empty,
                SegmentCase = i => Case.Capital
            }
        );
        
        _routePathByClass = convertedName[0];
        
        Name = convertedName[1];
    }

    public virtual string RoutePath => _routePathByClass;

    private string[] GetPathByClass(params ConventionDescriptor[] conventions)
    {
        var name = GetType().Name;
        var nameLower = name.ToLower();

        var tag = "requesthandler";

        if (nameLower.StartsWith(tag))
        {
            name = name.Substring(tag.Length, name.Length - tag.Length);

            nameLower = name.ToLower();
        }

        if (nameLower.EndsWith(tag))
        {
            name = name.Substring(0, name.Length - tag.Length);
        }

        var nc = new NamingConvention();

        var parsed = nc.Parse(name);

        if (parsed)
        {
            return conventions.Select(c => nc.Render(parsed.Value.Segments, c)).ToArray();
        }

        return conventions.Select(c => name).ToArray();
    }

    protected abstract Task PerformHandling();

    private HttpContext? _context;

    protected HttpContext HttpContext
    {
        get
        {
            if (_context is { } c)
            {
                return c;
            }

            throw new Exception($"HttpContext can not be reached outside the {nameof(PerformHandling)} method.");
        }
    }

    public Task Handle(HttpContext context)
    {
        _context = context;

        return PerformHandling();
    }


    protected T Inject<T>()
    {
        var service = HttpContext.RequestServices.GetService<T>();

        if (service is { } s) return s;

        throw new Exception($"Please add service registration for the type: {typeof(T).FullName}");
    }


    protected async Task<T?> ReadRequestBody<T>()
    {
        try
        {
            var value = await HttpContext.Request.ReadFromJsonAsync<T>();

            return value;
        }
        catch
        {
            /**/
        }

        return default;
    }


    protected async Task<List<UploadedFile>> ReadUploadedFiles()
    {
        var files = new List<UploadedFile>();

        foreach (var formFile in HttpContext.Request.Form.Files)
        {
            files.Add(new UploadedFile(
                formFile.FileName,
                formFile.Name,
                await formFile.OpenReadStream().ReadAsBytesArrayAsync(formFile.Length),
                formFile.Length
            ));
        }

        return files;
    }

    protected Task Ok(object response)
    {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

        return HttpContext.Response.WriteAsJsonAsync(response);
    }

    protected Task Ok()
    {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

        return Task.CompletedTask;
    }

    protected Task BadRequest(object response)
    {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return HttpContext.Response.WriteAsJsonAsync(response);
    }

    protected Task BadRequest()
    {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        return Task.CompletedTask;
    }
}