using System.Net;
using Acidmanic.Utilities.MintGum.Extensions;
using Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;
using Acidmanic.Utilities.MintGum.RequestHandlers.Extensions;
using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

internal abstract class RequestHandlerBase : IHttpRequestHandler, IRequestDescriptor
{
    public virtual HttpMethod Method => HttpMethod.Get;

    public string NameKebabCase { get; }
    public string NameTitleCase { get; }


    public string MethodName => Method.Method;
    
    public virtual string RoutePath => NameKebabCase;

    public string Uri => RoutePath;

    public virtual string Description { get; } = string.Empty;

    protected record UploadedFile(string FileName, string FormField, byte[] FileData, long Length);


    public RequestHandlerBase()
    {
        var convertedName = GetType().ReCase(
            "requesthandler",
            ConventionDescriptor.Standard.Kebab,
            ExtraNameConventions.Title
        );
        
        NameKebabCase = convertedName[0];
        
        NameTitleCase = convertedName[1];

        Description = convertedName[1] + " Endpoint";
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

    public IRequestDescriptor? GetDescriptor()
    {
        return this;
    }


    protected T Inject<T>()
    {
        var service = HttpContext.RequestServices.GetService<T>();

        if (service is { } s) return s;

        throw new Exception($"Please add service registration for the type: {typeof(T).FullName}");
    }

    protected string BaseUrl
    {
        get
        {
            var baseurl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;


            if (baseurl.EndsWith("/") || baseurl.EndsWith("\\"))
            {
                baseurl = baseurl.Substring(0, baseurl.Length - 1);
            }

            return baseurl;
        }
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