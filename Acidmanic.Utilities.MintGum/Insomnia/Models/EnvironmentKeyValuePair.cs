namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public class EnvironmentKeyValuePair
{
    public EnvironmentKeyValuePair(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; }
    
    public string Value { get; }

    public string Variable => "{{ _." + Key + " }}";

    public static implicit operator string(EnvironmentKeyValuePair value) => value.Variable;


    public override string ToString() => Variable;
}