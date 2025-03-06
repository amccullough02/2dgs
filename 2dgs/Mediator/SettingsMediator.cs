using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

/// <summary>
/// A mediator class that manages communication between the settings scene, settings user interface, and settings save file.
/// </summary>
public class SettingsMediator
{
    /// <summary>
    /// The current resolution of the application.
    /// </summary>
    public Vector2 CurrentResolution { get; set; }
    /// <summary>
    /// Signifies if the application is remapping a shortcut.
    /// </summary>
    public bool Remapping { get; set; }
    /// <summary>
    /// Signifies if a shortcut's list of keys should be cleared.
    /// </summary>
    public bool ClearShortcut { get; set; }
    /// <summary>
    /// Signifies if all shortcuts should be reset to their default values.
    /// </summary>
    public bool ResetShortcuts { get; set; }
    /// <summary>
    /// Provides a preview of the new shortcut before it is saved.
    /// </summary>
    public string ShortcutPreview = "";
    /// <summary>
    /// Used to check which shortcut is currently being modified.
    /// </summary>
    public string WhichShortcut = "";
    /// <summary>
    /// A dictionary containing the new keyboard shortcuts to save.
    /// </summary>
    public Dictionary<string, List<Keys>> NewShortcuts = new();
    /// <summary>
    /// A dictionary containing the default keyboard shortcuts.
    /// </summary>
    public Dictionary<string, List<Keys>> DefaultShortcuts = new();

    /// <summary>
    /// Empties the NewShortcuts dictionary, useful when a new remapping process begins to prevent conflicts.
    /// </summary>
    public void ResetNewShortcuts()
    {
        foreach (var key in NewShortcuts.Keys)
        {
            NewShortcuts[key].Clear();
        }
    }
}