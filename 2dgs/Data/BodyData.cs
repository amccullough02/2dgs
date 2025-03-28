using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace _2dgs;

/// <summary>
/// A data class used for interop between JSON.NET and a 2DGS Body object.
/// </summary>
public class BodyData
{
    /// <summary>
    /// The body's name.
    /// </summary>
    public string Name;
    /// <summary>
    /// The body's position.
    /// </summary>
    public Vector2 Position { get; set; }
    /// <summary>
    /// The body's velocity.
    /// </summary>
    public Vector2 Velocity { get; set; }
    /// <summary>
    /// The body's mass.
    /// </summary>
    public float Mass { get; set; }
    /// <summary>
    /// The body's display size.
    /// </summary>
    public float DisplaySize { get; set; }
    /// <summary>
    /// The body's color.
    /// </summary>
    [JsonIgnore]
    public Color Color { get; set; }

    /// <summary>
    /// Converts a body's color to/from RGB to/from hexadecimal.
    /// </summary>
    [JsonProperty("Color")]
    public string ColorString
    {
        get => $"#{Color.R:X2}{Color.G:X2}{Color.B:X2}";
        set => Color = ParseHexColor(value);
    }

    /// <summary>
    /// Converts a hexadecimal color string to an XNA RGB Color object.
    /// </summary>
    /// <param name="hex">The hexadecimal representation of the body's color.</param>
    /// <returns>An XNA RGB Color object.</returns>
    private static Color ParseHexColor(string hex)
    {
        var r = Convert.ToInt32(hex.Substring(1, 2), 16);
        var g = Convert.ToInt32(hex.Substring(3, 2), 16);
        var b = Convert.ToInt32(hex.Substring(5, 2), 16);
        
        return new Color(r, g, b);
    }
}