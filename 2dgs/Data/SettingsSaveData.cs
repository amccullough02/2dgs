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
    public List<Keys> NamesShortcut { get; set; } = [];
    public List<Keys> GlowShortcut { get; set; } = [];
    public List<Keys> EditShortcut { get; set; } = [];
    public List<Keys> ScreenshotShortcut { get; set; } = [];
}