using System;
using System.IO;
using Newtonsoft.Json;

namespace _2dgs;

/// <summary>
/// A class to perform read and write operations to the simulation and user settings files.
/// </summary>
public class SaveSystem
{
    /// <summary>
    /// The default path of the user settings file.
    /// </summary>
    private const string DefaultUserSettings = "../../../savedata/user_settings.json";
    
    /// <summary>
    /// Reads from a simulation file.
    /// </summary>
    /// <param name="path">The path of the simulation file.</param>
    /// <returns>A SimulationSaveData object containing the save data.</returns>
    public SimulationSaveData LoadSimulation(string path)
    {
        if (File.Exists(path))
        {
            var jsonData = File.ReadAllText(path);
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector2Converter());
            
            return JsonConvert.DeserializeObject<SimulationSaveData>(jsonData, settings);
        }

        return new SimulationSaveData();
    }

    /// <summary>
    /// Reads from the user settings file.
    /// </summary>
    /// <returns>A SettingsSaveData object containing data from the user settings file.</returns>
    public SettingsSaveData LoadSettings()
    {
        var jsonData = File.ReadAllText(DefaultUserSettings);
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new KeyEnumConvertor());
        return JsonConvert.DeserializeObject<SettingsSaveData>(jsonData, settings);
    }
    
    /// <summary>
    /// Writes to a simulation file.
    /// </summary>
    /// <param name="path">The path of the simulation file.</param>
    /// <param name="simulationSaveData">The data to write.</param>
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

    /// <summary>
    /// Writes to the user settings file.
    /// </summary>
    /// <param name="settingsSaveData">The data to write.</param>
    public void SaveSettings(SettingsSaveData settingsSaveData)
    {
        try
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;
            settings.Converters.Add(new KeyEnumConvertor());
            var jsonData = JsonConvert.SerializeObject(settingsSaveData, settings);

            File.WriteAllText(DefaultUserSettings, jsonData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}