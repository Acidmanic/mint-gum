namespace Acidmanic.Utilities.MintGum.Insomnia.Models;

public class InsomniaWorkspace:InsomniaResource
{
    protected override string GetResourceType => "workspace";
    protected override string GetResourceTypeShort => "wrk";

    public string Scope => "design";
    
    public string Name { get; set; } = string.Empty;
}