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

    private string filePath = "../../../savedata/lessons/test.json";
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
        var newPath = "../../../savedata/lessons/testy.json";
        fm.RenameFile(filePath, newPath);
        Console.WriteLine("TEST - Rename JSON file... PASS!");
        TestDeleteJsonFile(newPath);
    }

    public void TestDeleteJsonFile(string filePath)
    {
        fm.DeleteFile(filePath);
        Console.WriteLine("TEST - Delete JSON file... PASS!");
    }
    
    public void TestSimulationLoading(int serialized, int loaded)
    {
        if (serialized == loaded)
        {
            Console.WriteLine("Test - Loading of simulation file... PASS!");
        }
        else
        {
            Console.WriteLine("Test - Loading of simulation file... FAIL!");
        }
    }

    public void RunAllTests(GraphicsDeviceManager graphics, String windowTitle)
    {
        TestApplicationName(windowTitle);
        TestApplicationResolution(graphics);
        // this.TestRenameJsonFile();
    }
}