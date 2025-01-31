using System;
using System.IO;
using Newtonsoft.Json;

namespace _2dgs;

public class SaveSystem
{
    public void CreateBlankSimulation(string saveFilePath)
    {
        var saveData = new SaveData();
        
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            settings.Formatting = Formatting.Indented;
            var jsonData = JsonConvert.SerializeObject(saveData, settings);

            File.WriteAllText(saveFilePath, jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public void Save(string saveFilePath, SaveData saveData)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            settings.Formatting = Formatting.Indented;
            var jsonData = JsonConvert.SerializeObject(saveData, settings);

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
            var jsonData = File.ReadAllText(saveFilePath);
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            
            return JsonConvert.DeserializeObject<SaveData>(jsonData, settings);
        }

        return null;
    }
}