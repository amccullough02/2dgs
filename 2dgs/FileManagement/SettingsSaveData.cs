using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsSaveData
{
    public int VerticalResolution = 1080;
    public int HorizontalResolution = 1920;
    public bool Fullscreen = false;
    public List<Keys> PauseShortcut = [];
    public List<Keys> SpeedUpShortcut = [];
    public List<Keys> SpeedDownShortcut = [];
    public List<Keys> TrailsShortcut = [];
    public List<Keys> NamesShortcut = [];
    public List<Keys> GlowShortcut = [];
    public List<Keys> EditShortcut = [];
    public List<Keys> ScreenshotShortcut = [];
}