using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

/// <summary>
/// A class used to associate various custom string operations that 2DGS requires.
/// </summary>
public static class StringTransformer
{
    /// <summary>
    /// Extracts and transforms a filename to Pascal Case, with underscores replaced by spaces.
    /// </summary>
    /// <param name="filename">The filename to transform.</param>
    /// <returns></returns>
    public static string FileNamePrettier(string filename)
    {
        var textInfo = CultureInfo.InvariantCulture.TextInfo;
        return string.Join(" ", filename.Split('_'))
            .ToLowerInvariant()
            .Split(' ')
            .Select(word => textInfo.ToTitleCase(word))
            .Aggregate((x, y) => $"{x} {y}");
    }

    /// <summary>
    /// Returns a human-readable representation of a keyboard shortcut.
    /// </summary>
    /// <param name="keys">The keyboard shortcut.</param>
    /// <returns></returns>
    public static string KeyBindString(List<Keys> keys)
    {
        var keyBindString = "";

        for (var i = 0; i < keys.Count; i++)
        {
            keyBindString += keys[i].ToString();

            if (i != keys.Count - 1)
            {
                keyBindString += ", ";
            }
        }
        
        return keyBindString;
    }
}