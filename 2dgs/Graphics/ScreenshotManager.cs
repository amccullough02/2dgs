using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public static class ScreenshotManager
{
    public static void Capture(GraphicsDevice graphicsDevice)
    {
        int width = graphicsDevice.PresentationParameters.BackBufferWidth;
        int height = graphicsDevice.PresentationParameters.BackBufferHeight;
        
        Texture2D screenshot = new Texture2D(graphicsDevice, width, height);
        Color[] data = new Color[width * height];
        graphicsDevice.GetBackBufferData(data);
        screenshot.SetData(data);
        
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
            $"screenshot_{DateTime.Now:yyyyMmdd_HHmmss}.png");

        using FileStream stream = new FileStream(path, FileMode.Create);
        screenshot.SaveAsPng(stream, width, height);
        Console.WriteLine("DEBUG: Screenshot saved to " + path);
        screenshot.Dispose();
    }
}