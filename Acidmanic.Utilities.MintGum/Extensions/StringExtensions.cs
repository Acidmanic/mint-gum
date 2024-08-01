namespace Acidmanic.Utilities.MintGum.Extensions;

internal static class StringExtensions
{


    public static string JoinPath(this string path, params string[] segments) =>
        JoinAsPath(path, segments);

    public static string JoinAsPath(params string[] segments) => JoinAsPath(string.Empty, segments);
    
    private static string JoinAsPath(string firstValue, params string[] segments)
    {
        var result = firstValue;

        foreach (var segment in segments)
        {
            var delimmiter = "";
            
            if (!result.EndsWith("/") && !segment.StartsWith("/"))
            {
                delimmiter = "/";
            }

            result += delimmiter + segment;
        }

        return result;
    }
}