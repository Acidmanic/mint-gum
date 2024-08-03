namespace Acidmanic.Utilities.MintGum.RequestHandling.Contracts;

public interface ISuggestedValueFactory<T>
{
    T Suggest(IRequestDescriptor descriptor);
}