using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A static class used to provide a facade for capturing in-game screenshots.
/// </summary>
public static class ScreenshotManager
{
    /// <summary>
    /// A method that captures an in-game screenshot which is then saved to the user's My Pictures folder (Windows is assumed).
    /// </summary>
    /// <param name="graphicsDevice">A reference to MonoGame's GraphicsDevice class.</param>
    public static void Capture(GraphicsDevice graphicsDevice)
    {
        var width = graphicsDevice.PresentationParameters.BackBufferWidth;
        var height = graphicsDevice.PresentationParameters.BackBufferHeight;
        
        var screenshot = new Texture2D(graphicsDevice, width, height);
        var data = new Color[width * height];
        graphicsDevice.GetBackBufferData(data);
        screenshot.SetData(data);
        
        if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "2D Gravity Simulator")))
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "2D Gravity Simulator"));
        }
        
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), 
            $"2D Gravity Simulator\\screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");

        var stream = new FileStream(path, FileMode.Create);
        screenshot.SaveAsPng(stream, width, height);
        Console.WriteLine("DEBUG: Screenshot saved to " + path);
        screenshot.Dispose();
    }
}