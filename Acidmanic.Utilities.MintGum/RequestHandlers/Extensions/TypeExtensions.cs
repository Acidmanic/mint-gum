using Acidmanic.Utilities.NamingConventions;

namespace Acidmanic.Utilities.MintGum.RequestHandlers.Extensions;

internal static class TypeExtensions
{
    public static string[] ReCase(this Type type, string? suffix = null, params ConventionDescriptor[] conventions)
    {
        var name = type.Name;
        var nameLower = name.ToLower();


        if (suffix is { } tag)
        {
            tag = tag.ToLower();

            if (nameLower.StartsWith(tag))
            {
                name = name.Substring(tag.Length, name.Length - tag.Length);

                nameLower = name.ToLower();
            }

            if (nameLower.EndsWith(tag))
            {
                name = name.Substring(0, name.Length - tag.Length);
            }
        }

        var nc = new NamingConvention();

        var parsed = nc.Parse(name);

        if (parsed)
        {
            return conventions.Select(c => nc.Render(parsed.Value.Segments, c)).ToArray();
        }

        return conventions.Select(c => name).ToArray();
    }
}