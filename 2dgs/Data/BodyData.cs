using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace _2dgs;

public class BodyData
{
    public string Name;
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Mass { get; set; }
    public float DisplayRadius { get; set; }
    [JsonIgnore]
    public Color Color { get; set; }

    [JsonProperty("Color")]
    public string ColorString
    {
        get => $"#{Color.R:X2}{Color.G:X2}{Color.B:X2}";
        set => Color = ParseHexColor(value);
    }

    private static Color ParseHexColor(string hex)
    {
        int r = Convert.ToInt32(hex.Substring(1, 2), 16);
        int g = Convert.ToInt32(hex.Substring(3, 2), 16);
        int b = Convert.ToInt32(hex.Substring(5, 2), 16);
        
        return new Color(r, g, b);
    }
}