using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsMenuData
{
    public bool Remapping { get; set; }
    public List<Keys> NewShortcut { get; set; }
}