using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2dgs;

/// <summary>
/// A custom JsonConverter for converting a MonoGame Vector2 object to a JSON object containing floating point values.
/// </summary>
public class Vector2Converter : JsonConverter<Vector2>
{
    /// <summary>
    /// Writes the Vector2 object to a JSON object containing a value for X and Y.
    /// </summary>
    /// <param name="writer">The JSON.NET writer.</param>
    /// <param name="value">The value to be written.</param>
    /// <param name="serializer">Controls JSON encoding.</param>
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("X");
        writer.WriteValue(value.X);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.Y);
        writer.WriteEndObject();
    }

    /// <summary>
    /// Reads from the JSON object and converts back to a Vector2 object.
    /// </summary>
    /// <param name="reader">The JSON.NET reader.</param>
    /// <param name="objectType">A generic type.</param>
    /// <param name="existingValue">The value being read from.</param>
    /// <param name="hasExistingValue">If the value being read from exists.</param>
    /// <param name="serializer">Controls JSON encoding.</param>
    /// <returns>A MonoGame Vector2 object.</returns>
    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        
        var x = obj["X"].Value<float>();
        var y = obj["Y"].Value<float>();

        return new Vector2(x, y);
    }
}