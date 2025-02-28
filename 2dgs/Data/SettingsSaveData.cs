using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

/// <summary>
/// A data class used to store data loaded from a user settings file.
/// </summary>
public class SettingsSaveData
{
    /// <summary>
    /// The vertical resolution of the application.
    /// </summary>
    public int VerticalResolution { get; set; } = 1080;
    /// <summary>
    /// The horizontal resolution of the application.
    /// </summary>
    public int HorizontalResolution { get; set; } = 1920;
    /// <summary>
    /// The application's display mode.
    /// </summary>
    public bool Fullscreen { get; set; }
    /// <summary>
    /// The shortcut used for pausing a simulation.
    /// </summary>
    public List<Keys> PauseShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to increase the timestep of a simulation.
    /// </summary>
    public List<Keys> SpeedUpShortcut { get; set; } = [];
    /// <summary>
    /// THe shortcut used to decrease the timestep of a simulation.
    /// </summary>
    public List<Keys> SpeedDownShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to toggle body trails.
    /// </summary>
    public List<Keys> TrailsShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to toggle body orbits.
    /// </summary>
    public List<Keys> OrbitsShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to toggle body vectors.
    /// </summary>
    public List<Keys> VectorsShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to toggle body names.
    /// </summary>
    public List<Keys> NamesShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to toggle the body glow effect.
    /// </summary>
    public List<Keys> GlowShortcut { get; set; } = [];
    /// <summary>
    /// AThe shortcut used to toggle edit mode on or off.
    /// </summary>
    public List<Keys> EditShortcut { get; set; } = [];
    /// <summary>
    /// The shortcut used to take an in-game screenshot.
    /// </summary>
    public List<Keys> ScreenshotShortcut { get; set; } = [];

    /// <summary>
    /// Refreshes the contents of the data class. This is required to prevent stale references when the user configures new keyboard shortcuts.
    /// </summary>
    /// <param name="saveSystem">A reference to a 2DGS SaveSystem object.</param>
    public void Refresh(SaveSystem saveSystem)
    {
        var newSaveData = saveSystem.LoadSettings();
        
        VerticalResolution = newSaveData.VerticalResolution;
        HorizontalResolution = newSaveData.HorizontalResolution;
        Fullscreen = newSaveData.Fullscreen;
        PauseShortcut = newSaveData.PauseShortcut;
        SpeedUpShortcut = newSaveData.SpeedUpShortcut;
        SpeedDownShortcut = newSaveData.SpeedDownShortcut;
        TrailsShortcut = newSaveData.TrailsShortcut;
        OrbitsShortcut = newSaveData.OrbitsShortcut;
        VectorsShortcut = newSaveData.VectorsShortcut;
        NamesShortcut = newSaveData.NamesShortcut;
        GlowShortcut = newSaveData.GlowShortcut;
        EditShortcut = newSaveData.EditShortcut;
        ScreenshotShortcut = newSaveData.ScreenshotShortcut;
    }
}