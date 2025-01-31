using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public static class KeyManager
{
    public static void Shortcut(List<Keys> keys, KeyboardState current, KeyboardState previous, Action action)
    {
        bool isPressed = keys.TrueForAll(k => current.IsKeyDown(k));
        bool wasPPressed = keys.TrueForAll(k => previous.IsKeyDown(k));
        
        if (isPressed && !wasPPressed) {
            action();
        }
    }
}