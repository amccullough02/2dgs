using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsSceneData
{
    public Vector2 CurrentResolution { get; set; }
    public bool Remapping { get; set; }
    public bool ClearShortcut { get; set; }
    public bool ResetShortcuts { get; set; }
    public string ShortcutPreview = "";
    public string WhichShortcut = "";
    public Dictionary<string, List<Keys>> NewShortcuts = new();
    public Dictionary<string, List<Keys>> DefaultShortcuts = new();

    public void ResetNewShortcuts()
    {
        foreach (var key in NewShortcuts.Keys)
        {
            NewShortcuts[key].Clear();
        }
    }
}