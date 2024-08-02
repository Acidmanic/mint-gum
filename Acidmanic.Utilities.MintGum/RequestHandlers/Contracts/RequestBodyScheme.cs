namespace Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

public class RequestBodyScheme
{
    public RequestBodyMimeType MimeType { get; set; } = RequestBodyMimeType.None;
    
    public Type? BodyModelType { get; set; }

    public List<MultiPartKeyValuePair> MultiPartData { get; } = new();

    public List<UrlEncodedKeyValuePair> UrlEncodeData { get; } = new();

}