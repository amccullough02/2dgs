using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsMenuData
{
    public bool Remapping { get; set; }
    public bool ClearShortcut { get; set; }
    public string ShortcutPreview = "";
    public List<Keys> NewShortcut = [];
}