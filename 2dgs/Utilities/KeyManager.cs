using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

/// <summary>
/// A class used to aid in the execution of keyboard shortcuts.
/// </summary>
public static class KeyManager
{
    /// <summary>
    /// A method used to encapsulate the conditions for a keyboard shortcut. Two separate keyboard states are required to prevent 'repeated' actions if the keys
    /// are held down for more than a single frame. Instead, a key (or keys) will need to be released before the shortcut can be used again.
    /// </summary>
    /// <param name="keys">The keys required to activate the shortcut.</param>
    /// <param name="current">The keys that are currently being held down.</param>
    /// <param name="previous">The keys that were just held down a previous frame.</param>
    /// <param name="action">The action to execute upon satisfying the conditions of the shortcut.</param>
    public static void Shortcut(List<Keys> keys, KeyboardState current, KeyboardState previous, Action action)
    {
        var isPressed = keys.TrueForAll(k => current.IsKeyDown(k));
        var wasPPressed = keys.TrueForAll(k => previous.IsKeyDown(k));
        
        if (isPressed && !wasPPressed) {
            action();
        }
    }
}