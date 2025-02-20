using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class TestRunner
{
    private static List<string> _results = [];
    private static bool _testingComplete;
    private const string ResultsPath = "../../../savedata/test/results.txt";

    public static void AssertApplicationName(string actualName, string expectedName)
    {
        if (actualName != expectedName)
        {
            const string result = "TEST #1: Application name is not as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #1: Application name is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }   
    }

    public static void AssertApplicationResolution(GraphicsDeviceManager graphics, SettingsSaveData settingsSaveData)
    {
        var actualDisplayWidth = graphics.PreferredBackBufferWidth;
        var actualDisplayHeight = graphics.PreferredBackBufferHeight;
        var expectedDisplayWidth = settingsSaveData.HorizontalResolution;
        var expectedDisplayHeight = settingsSaveData.VerticalResolution;

        if (actualDisplayWidth != expectedDisplayWidth || actualDisplayHeight != expectedDisplayHeight)
        {
            const string result = "TEST #2: Application resolution is as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #2: Application resolution is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertApplicationDisplayMode(GraphicsDeviceManager graphics, SettingsSaveData settingsSaveData)
    {
        var actualDisplayMode = graphics.IsFullScreen;
        var expectedDisplayMode = settingsSaveData.Fullscreen;

        if (actualDisplayMode != expectedDisplayMode)
        {
            const string result = "TEST #3: Application display mode is as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #3: Application display mode is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertLessonsLoaded(ListView listView, string folderPath)
    {
        var numOfWidgets = listView.Widgets.Count;
        var numOfLessons = 0;

        try
        {
            numOfLessons = Directory.GetFiles(folderPath, "*.json").Length;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (numOfWidgets != numOfLessons)
        {
            const string result = "Test #4: Lesson files loaded (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "Test #4: Lesson files loaded (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }
    
    public static void AssertUserSimsLoaded(ListView listView, string folderPath)
    {
        var numOfWidgets = listView.Widgets.Count;
        var numOfLessons = 0;

        try
        {
            numOfLessons = Directory.GetFiles(folderPath, "*.json").Length;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (numOfWidgets != numOfLessons)
        {
            const string result = "Test #5: Sandbox files loaded (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "Test #5: Sandbox files loaded (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        
        _testingComplete = true;
    }

    public static void SaveResults()
    {
        if (!_testingComplete) return;

        try
        {
            using var writer = new StreamWriter(ResultsPath);
            writer.WriteLine($"=== TESTING RESULTS for {DateTime.Now.ToUniversalTime()} ===");
            foreach (var result in _results)
            {
                writer.WriteLine(result);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _testingComplete = false;
    }
}