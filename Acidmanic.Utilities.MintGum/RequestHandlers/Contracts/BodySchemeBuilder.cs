using Acidmanic.Utilities.Reflection;

namespace Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

public class BodySchemeBuilder
{


    private RequestBodyScheme _scheme;

    public BodySchemeBuilder()
    {
        Clear();
    }

    public void Clear()
    {
        _scheme = new RequestBodyScheme(){MimeType = RequestBodyMimeType.None};
    }

    public BodySchemeBuilder SetJsonBody(Type type)
    {
        _scheme.MimeType = RequestBodyMimeType.Json;

        _scheme.BodyModelType = type;

        return this;
    }
    
    public BodySchemeBuilder SetXmlBody(Type type)
    {
        _scheme.MimeType = RequestBodyMimeType.Xml;

        _scheme.BodyModelType = type;

        return this;
    }
    
    public BodySchemeBuilder AddUrlEncoded(UrlEncodedValueType type, string key, string value)
    {
        _scheme.MimeType = RequestBodyMimeType.UrlEncodedForm;

        _scheme.UrlEncodeData.Add(new UrlEncodedKeyValuePair()
        {
            Name = key,
            Value = value,
            Type = type
        });

        return this;
    }
    
    public BodySchemeBuilder AddMultipart(MultipartValueType type, string key, string value)
    {
        _scheme.MimeType = RequestBodyMimeType.MultipartForm;

        _scheme.MultiPartData.Add(new MultiPartKeyValuePair()
        {
            Name = key,
            Value = value,
            Type = type
        });

        return this;
    }


    public RequestBodyScheme Build()
    {
        if (_scheme.MimeType != RequestBodyMimeType.Json || _scheme.MimeType != RequestBodyMimeType.Xml){
            
            _scheme.BodyModelType = null;
        }
        if(_scheme.MimeType != RequestBodyMimeType.MultipartForm) _scheme.MultiPartData.Clear();
        
        if(_scheme.MimeType != RequestBodyMimeType.UrlEncodedForm) _scheme.UrlEncodeData.Clear();

        return _scheme;
    }
    
}