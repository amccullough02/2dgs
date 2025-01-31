using System.Globalization;
using System.Linq;

namespace _2dgs;

public static class StringTransformer
{
    public static string FileNamePrettier(string filename)
    {
        var textInfo = CultureInfo.InvariantCulture.TextInfo;
        return string.Join(" ", filename.Split('_'))
            .ToLowerInvariant()
            .Split(' ')
            .Select(word => textInfo.ToTitleCase(word))
            .Aggregate((x, y) => $"{x} {y}");
    }
}