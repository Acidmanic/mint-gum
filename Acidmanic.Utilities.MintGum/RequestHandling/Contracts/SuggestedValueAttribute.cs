using Acidmanic.Utilities.Reflection;

namespace Acidmanic.Utilities.MintGum.RequestHandling.Contracts;


[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class SuggestedValueAttribute:Attribute
{
    
    public SuggestedValueStrategy Strategy { get; }
    
    private Type Type { get; }
    
    private object Value { get; }


    public SuggestedValueAttribute(Type factoryType)
    {
        if (factoryType.IsAbstract || !TypeCheck.IsSpecificOf(factoryType, typeof(ISuggestedValueFactory<>)))
        {
            throw new Exception($"Factory Type Must be an concrete implementation of {typeof(ISuggestedValueFactory<>).Name}");
        }

        Strategy = SuggestedValueStrategy.DiRegisteredFactory;

        Value = new object();

        Type = factoryType;
    }

    public SuggestedValueAttribute(object value)
    {
        Strategy = SuggestedValueStrategy.FixedValue;

        Value = value;

        Type = typeof(object);
    }

    public T? SuggestValue<T>(IServiceProvider sp,IRequestDescriptor requestDescriptor) where T : class
    {
        if (Strategy == SuggestedValueStrategy.FixedValue)
        {
            return Value as T;
        }

        var factory = sp.GetService(Type) as ISuggestedValueFactory<T>;

        if (factory is { } f) return f.Suggest(requestDescriptor);
        
        return default;
    }


    public T? SuggestValue<T>(IRequestDescriptor requestDescriptor) where T : class
    {
        if (Strategy == SuggestedValueStrategy.FixedValue)
        {
            return Value as T;
        }

        return default;
    }
}