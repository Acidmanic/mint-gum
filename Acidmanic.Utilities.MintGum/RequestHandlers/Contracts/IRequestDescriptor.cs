namespace Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

public interface IRequestDescriptor
{
    public string NameKebabCase { get; }
    
    public string NameTitleCase { get; }
    
    string MethodName { get; }
    
    string Uri { get; }
}