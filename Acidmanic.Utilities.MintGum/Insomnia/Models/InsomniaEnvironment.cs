using System.Text.Json.Serialization;
using Acidmanic.Utilities.Filtering.Models;
using Newtonsoft.Json;

namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public class InsomniaEnvironment:InsomniaResource
{
    protected override string GetResourceType => "environment";
    protected override string GetResourceTypeShort => "env";

    public string Name { get; set; } = string.Empty;

    public bool IsPrivate => false;

    public Dictionary<string, string> Data { get; } = new ();

    public class OrderList
    {
        [JsonProperty("&")]
        [JsonPropertyName("&")]
        public List<string> Orders { get; set; } = new List<string>();
    };

    public OrderList DataPropertyOrder { get; } = new OrderList();

    public object? Color => null;


    public void Add(EnvironmentKeyValuePair value) => Add(value.Key, value.Value);
    
    public void Add(string key, string value)
    {
        if (Data.ContainsKey(key))
        {
            Data.Remove(key);

            DataPropertyOrder.Orders.RemoveAll(a => a == key);
            
        }
        Data.Add(key,value);
        
        DataPropertyOrder.Orders.Add(key);
    }

}