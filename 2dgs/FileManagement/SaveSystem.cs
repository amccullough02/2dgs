using System;
using System.IO;
using Newtonsoft.Json;

namespace _2dgs;

public class SaveSystem
{
    private string userSettings = "../../../savedata/user_settings.json";
    
    public void CreateBlankSimulation(string saveFilePath)
    {
        var saveData = new SimulationSaveData();
        
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
    
    public SimulationSaveData LoadSimulation(string path)
    {
        if (File.Exists(path))
        {
            var jsonData = File.ReadAllText(path);
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            
            return JsonConvert.DeserializeObject<SimulationSaveData>(jsonData, settings);
        }

        return null;
    }

    public SettingsSaveData LoadSettings()
    {
        var jsonData = File.ReadAllText(userSettings);
        return JsonConvert.DeserializeObject<SettingsSaveData>(jsonData);
    }
    
    public void SaveSimulation(string path, SimulationSaveData simulationSaveData)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            settings.Formatting = Formatting.Indented;
            var jsonData = JsonConvert.SerializeObject(simulationSaveData, settings);

            File.WriteAllText(path, jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void SaveSettings(SettingsSaveData settingsSaveData)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            var jsonData = JsonConvert.SerializeObject(settingsSaveData, settings);

            File.WriteAllText(userSettings, jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}