using System;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public static class KeyManager
{
    public static void Shortcut(Keys key1, Keys key2, KeyboardState current, KeyboardState previous, Action action)
    {
        bool isPressed = current.IsKeyDown(key1) && current.IsKeyDown(key2);
        bool wasPPressed = previous.IsKeyDown(key1) && previous.IsKeyDown(key2);
        
        if (isPressed && !wasPPressed) {
            action();
        }
    }
}