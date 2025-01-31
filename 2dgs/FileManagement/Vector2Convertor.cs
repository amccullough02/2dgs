using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _2dgs;

public class Vector2Converter : JsonConverter<Vector2>
{
    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("X");
        writer.WriteValue(value.X);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.Y);
        writer.WriteEndObject();
    }

    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        
        var x = obj["X"].Value<float>();
        var y = obj["Y"].Value<float>();

        return new Vector2(x, y);
    }
}