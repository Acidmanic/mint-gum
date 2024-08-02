namespace Acidmanic.Utilities.MintGum.RequestHandling.Contracts;

public class RequestBodyScheme
{
    public static readonly RequestBodyScheme None = new RequestBodyScheme()
    {
        MimeType = RequestBodyMimeType.None,
    };
    
    public RequestBodyMimeType MimeType { get; set; } = RequestBodyMimeType.None;
    
    public Type? BodyModelType { get; set; }

    public List<MultiPartKeyValuePair> MultiPartData { get; } = new();

    public List<UrlEncodedKeyValuePair> UrlEncodeData { get; } = new();

}