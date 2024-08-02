namespace Acidmanic.Utilities.MintGum.RequestHandlers.Contracts;

public interface IRequestDescriptor
{
    string NameKebabCase { get; }
    
    string NameTitleCase { get; }
    
    string MethodName { get; }
    
    string Uri { get; }
    
    string Description { get; } 
    
    
    Dictionary<string,string> Headers { get; }

}