using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2dgs;

public class KeyEnumConvertor : JsonConverter<List<Keys>>
{

    public override void WriteJson(JsonWriter writer, List<Keys> value, JsonSerializer serializer)
    {
        writer.WriteStartArray();

        foreach (var key in value)
        {
            writer.WriteValue(key.ToString());
        }
        
        writer.WriteEndArray();
    }
    
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