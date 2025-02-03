using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework.Input;

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

    public static string KeybindString(List<Keys> keys)
    {
        var keybindString = "";

        for (var i = 0; i < keys.Count; i++)
        {
            keybindString += keys[i].ToString();

            if (i != keys.Count - 1)
            {
                keybindString += ", ";
            }
        }
        
        return keybindString;
    }
}