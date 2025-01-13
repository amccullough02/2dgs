using System;
using System.IO;
using Newtonsoft.Json;

namespace _2dgs;

public class SaveSystem
{
    public void Save(string saveFilePath, SaveData saveData)
    {
        try
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            settings.Formatting = Formatting.Indented;
            string jsonData = JsonConvert.SerializeObject(saveData, settings);

            File.WriteAllText(saveFilePath, jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public SaveData Load(string saveFilePath)
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            
            return JsonConvert.DeserializeObject<SaveData>(jsonData, settings);
        }

        return null;
    }
}