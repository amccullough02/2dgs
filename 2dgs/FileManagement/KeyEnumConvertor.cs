using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2dgs;

/// <summary>
/// A custom JSONConvertor designed to convert MonoGame Keys to a string format than be serialized.
/// </summary>
public class KeyEnumConvertor : JsonConverter<List<Keys>>
{

    /// <summary>
    /// Writes the list of MonoGame keys as an array of strings.
    /// </summary>
    /// <param name="writer">The JSON.NET writer.</param>
    /// <param name="value">The value to be written.</param>
    /// <param name="serializer">Controls JSON encoding.</param>
    public override void WriteJson(JsonWriter writer, List<Keys> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();

        foreach (var key in value)
        {
            writer.WriteValue(key.ToString());
        }
        
        writer.WriteEndArray();
    }
    
    /// <summary>
    /// Reads the array of strings and converts it back into a list of MonoGame keys.
    /// </summary>
    /// <param name="reader">The JSON.NET reader.</param>
    /// <param name="objectType">A generic type.</param>
    /// <param name="existingValue">The value being read from.</param>
    /// <param name="hasExistingValue">If the value being read from exists.</param>
    /// <param name="serializer">Controls JSON encoding.</param>
    /// <returns>A list of MonoGame keys.</returns>
    public override List<Keys> ReadJson(JsonReader reader, Type objectType, List<Keys> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var obj = JArray.Load(reader);
        
        var keys = new List<Keys>();

        foreach (var key in obj)
        {
            keys.Add((Keys)Enum.Parse(typeof(Keys), key.ToString()));
        }
        
        return keys;
    }
}