using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            const string result = "TEST #4: Lesson files loaded (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #4: Lesson files loaded (PASS).";
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
            const string result = "TEST #5: Sandbox files loaded (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #5: Sandbox files loaded (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertFileRename(string oldPath, string newPath)
    {
        var oldPathExists = File.Exists(oldPath);
        var newPathExists = File.Exists(newPath);

        if (oldPathExists)
        {
            const string result = "TEST #6: File rename is as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        } else if (!newPathExists)
        {
            const string result = "TEST #6: File rename is as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #6: File rename is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertFileDeletion(string filePath)
    {
        if (File.Exists(filePath))
        {
            const string result = "TEST #7: File deletion as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #7: File deletion is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertBodiesDataIntegrity(List<Body> bodies, List<BodyData> bodiesData)
    {
        var numOfInstanceBodies = bodies.Count;
        var numOfSavedBodies = bodiesData.Count;

        if (numOfInstanceBodies != numOfSavedBodies)
        {
            const string result = "TEST #8: Bodies data integrity is as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #8: Bodies data integrity is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertLessonDataIntegrity(List<LessonPage> lessonPages, List<LessonPage> lessonData)
    {
        var numOfInstanceLessons = lessonPages.Count;
        var numOfSavedLessons = lessonData.Count;

        if (numOfInstanceLessons != numOfSavedLessons)
        {
            const string result = "TEST #9: Lesson data integrity is as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #9: Lesson data integrity is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertBodyCreated(List<Body> bodies, Body newBody)
    {
        if (bodies.Contains(newBody))
        {
            const string result = "TEST #10: Body created as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #10: Body created as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertBodyEdited(List<Body> bodies, Body editedBody)
    {
        if (bodies.Contains(editedBody))
        {
            const string result = "TEST #11: Body edited as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #11: Body edited as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    public static void AssertBodyDeleted(List<Body> bodies, Body deletedBody)
    {
        if (!bodies.Contains(deletedBody))
        {
            const string result = "TEST #12: Body deleted as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #12: Body deleted as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
    }

    private static bool CompareSaveData(SimulationSaveData instanceData, SimulationSaveData saveData)
    {
        var bodies = instanceData.Bodies.Count == saveData.Bodies.Count;
        var pages = instanceData.LessonPages.Count == saveData.LessonPages.Count;
            
        return bodies && pages;
    }

    public static void AssertSimulationSaved(string filePath, SimulationSaveData instanceData, SaveSystem saveSystem)
    {
        var saveData = saveSystem.LoadSimulation(filePath);
        
        if (File.Exists(filePath) && CompareSaveData(instanceData, saveData))
        {
            const string result = "TEST #13: Simulation file saved as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        else
        {
            const string result = "TEST #13: Simulation file saved as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
        }
        
        _testingComplete = true;
    }

    public static void SaveResults()
    {
        if (!_testingComplete) return;

        var uniqueResults = _results.Distinct().ToList();

        try
        {
            using var writer = new StreamWriter(ResultsPath);
            writer.WriteLine($"=== TESTING RESULTS for {DateTime.Now.ToUniversalTime()} ===");
            foreach (var result in uniqueResults)
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