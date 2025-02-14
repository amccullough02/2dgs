using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsSaveData
{
    public int VerticalResolution { get; set; } = 1080;
    public int HorizontalResolution { get; set; } = 1920;
    public bool Fullscreen { get; set; }
    public List<Keys> PauseShortcut { get; set; } = [];
    public List<Keys> SpeedUpShortcut { get; set; } = [];
    public List<Keys> SpeedDownShortcut { get; set; } = [];
    public List<Keys> TrailsShortcut { get; set; } = [];
    public List<Keys> OrbitsShortcut { get; set; } = [];
    public List<Keys> VectorsShortcut { get; set; } = [];
    public List<Keys> NamesShortcut { get; set; } = [];
    public List<Keys> GlowShortcut { get; set; } = [];
    public List<Keys> EditShortcut { get; set; } = [];
    public List<Keys> ScreenshotShortcut { get; set; } = [];

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
        NamesShortcut = newSaveData.NamesShortcut;
        GlowShortcut = newSaveData.GlowShortcut;
        EditShortcut = newSaveData.EditShortcut;
        ScreenshotShortcut = newSaveData.ScreenshotShortcut;
    }
}