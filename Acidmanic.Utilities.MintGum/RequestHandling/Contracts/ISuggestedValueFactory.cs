namespace Acidmanic.Utilities.MintGum.RequestHandling.Contracts;


public interface ISuggestedValueFactory
{
    object? Suggest(IRequestDescriptor descriptor);
}
