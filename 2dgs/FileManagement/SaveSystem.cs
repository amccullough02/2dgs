using System.IO;
using Newtonsoft.Json;

namespace _2dgs;

public class SaveSystem
{
    private string saveFilePath = "../../../savedata.json";

    public void Save(SaveData data)
    {
        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(saveFilePath, jsonData);
    }

    public SaveData Load()
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