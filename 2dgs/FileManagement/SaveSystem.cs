using System.IO;
using Newtonsoft.Json;

namespace _2dgs;

public class SaveSystem
{
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