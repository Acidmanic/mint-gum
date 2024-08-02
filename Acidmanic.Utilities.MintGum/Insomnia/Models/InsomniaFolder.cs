namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public class InsomniaFolder:InsomniaResource
{
    protected override string GetResourceType => "request_group";
    protected override string GetResourceTypeShort => "fld";


    public object Environment { get; set; } = new ();

    public object? EnvironmentPropertyOrder { get; set; } = null;

    public string Name { get; set; } = string.Empty;

}