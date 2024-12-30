using System;
using Microsoft.Xna.Framework;

namespace _2dgs;

public class Test
{
    public void TestApplicationName(String windowTitle)
    {
        if (windowTitle == "2DGS - Alpha")
        {
            Console.WriteLine("TEST - Application title is correct... PASS!");
        }
        else
        {
            Console.WriteLine("TEST - Application title is correct... FAIL!");
        }
    }

    public void TestApplicationResolution(GraphicsDeviceManager graphics)
    {
        if (graphics.PreferredBackBufferHeight == 1080 && graphics.PreferredBackBufferWidth == 1920)
        {
            Console.WriteLine("TEST - Application resolution is correct... PASS!");
        }
        else
        {
            Console.WriteLine("TEST - Application resolution is correct... FAIL!");
        }
    }

    public void RunAllTests(GraphicsDeviceManager graphics, String windowTitle)
    {
        this.TestApplicationName(windowTitle);
        this.TestApplicationResolution(graphics);
    }
}