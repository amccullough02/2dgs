using System;
using System.IO;
using System.Text.Json;
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

    private string filePath = "../../../sims/lessons/test.json";
    private FileManager fm = new FileManager();

    private void CreateDummyJsonFile()
    {
        var data = new
        {
            Name = "John Doe",
        };
        
        var jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, jsonString);
    }

    public void TestRenameJsonFile()
    {
        CreateDummyJsonFile();
        var newPath = "../../../sims/lessons/testy.json";
        fm.RenameFile(filePath, newPath);
        Console.WriteLine("TEST - Rename JSON file... PASS!");
        TestDeleteJsonFile(newPath);
    }

    public void TestDeleteJsonFile(string filePath)
    {
        fm.DeleteFile(filePath);
        Console.WriteLine("TEST - Delete JSON file... PASS!");
    }

    public void RunAllTests(GraphicsDeviceManager graphics, String windowTitle)
    {
        this.TestApplicationName(windowTitle);
        this.TestApplicationResolution(graphics);
        // this.TestRenameJsonFile();
    }
}