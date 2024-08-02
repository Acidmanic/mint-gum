using System.Text.Json.Serialization;
using Acidmanic.Utilities.DataTypes;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public abstract class InsomniaResource
{
    [JsonProperty("_id")]
    [JsonPropertyName("_id")]
    public string Id { get; set; } = string.Empty;

    public string? ParentId { get; set; } 
    public long Modified { get; set; }
    public long Created { get; set; }

    [JsonProperty("_type")]
    [JsonPropertyName("_type")]
    public virtual string Type => GetResourceType;

    public string Description { get; set; } = string.Empty;

    protected abstract string GetResourceType { get; }
    protected abstract string GetResourceTypeShort { get; }
    
    public long MetaSortKey { get; set; }

    public string CreateId()
    {
        return $"{GetResourceTypeShort.ToLower()}_{Guid.NewGuid().ToString("N").ToLower()}";
    }

    public InsomniaResource()
    {
        var now = TimeStamp.Now.TotalMilliSeconds;

        Created = now;
        Modified = now;
    }
    
}