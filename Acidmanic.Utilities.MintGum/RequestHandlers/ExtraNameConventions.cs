using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers;

public static class ExtraNameConventions
{


    public static readonly ConventionDescriptor Title = new ConventionDescriptor
    {
        Delimiter = " ",
        Name = "Title",
        Separation = Separation.ByDelimiter,
        PreFix = string.Empty,
        SegmentCase = _ => Case.Capital
    }; 

    public static ConventionDescriptor[] Items = new[]
    {
        Title
    };
}