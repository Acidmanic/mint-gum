namespace Acidmanic.Utilities.MintGum.RequestHandling.Contracts;

public class UrlEncodedKeyValuePair
{
    public UrlEncodedValueType Type { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}