using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A class used for unit testing the 2DGS application.
/// </summary>
public static class TestRunner
{
    /// <summary>
    /// A list for test results.
    /// </summary>
    private static List<string> _results = [];
    /// <summary>
    /// Marks when the testing process is complete, called in the final test.
    /// </summary>
    private static bool _testingComplete;
    /// <summary>
    /// The path for the test results file.
    /// </summary>
    private const string ResultsPath = "../../../savedata/test/results.txt";

    /// <summary>
    /// A method to test if the application name is as expected.
    /// </summary>
    /// <param name="actualName">The actual name of the application.</param>
    /// <param name="expectedName">The name the application should actually be.</param>
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

    /// <summary>
    /// A method to test if the application resolution matches the user-defined resolution in the settings save file.
    /// </summary>
    /// <param name="graphics">A reference to the MonoGame GraphicsDeviceManager, used to obtain the current resolution.</param>
    /// <param name="settingsSaveData">A reference to the SettingsSaveData class, used to obtain the intended resolution.</param>
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

    /// <summary>
    /// A method to test if the application is fullscreen or windowed, and if that matches what is noted in the settings save file.
    /// </summary>
    /// <param name="graphics">A reference to the MonoGame GraphicsDeviceManager, used to obtain the current display mode.</param>
    /// <param name="settingsSaveData">A reference to the SettingsSaveData class, used to obtain the intended display mode.</param>
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

    /// <summary>
    /// A test to check if the number of lesson simulations loaded is as expected.
    /// </summary>
    /// <param name="listView">The widget displaying the available lesson simulations.</param>
    /// <param name="folderPath">The folder in which the lesson simulations are located.</param>
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
    
    /// <summary>
    /// A test to check if the number of lesson sandbox loaded is as expected.
    /// </summary>
    /// <param name="listView">The widget displaying the available sandbox simulations.</param>
    /// <param name="folderPath">The folder in which the sandbox simulations are located.</param>
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

    /// <summary>
    /// A method to test if the file has been renamed.
    /// </summary>
    /// <param name="oldPath">The path where the file was located.</param>
    /// <param name="newPath">The new path of the file.</param>
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

    /// <summary>
    /// A method to test if the file has been deleted.
    /// </summary>
    /// <param name="filePath">The path where the deleted was prior to deletion.</param>
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

    /// <summary>
    /// A method to test if the bodies have been loaded correctly.
    /// </summary>
    /// <param name="bodies">The bodies within into the simulation.</param>
    /// <param name="bodiesData">The bodies stored in the save file, used for the sake of comparison.</param>
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

    /// <summary>
    /// A method to test if the lesson content has been loaded correctly.
    /// </summary>
    /// <param name="lessonPages">The instantiated list of lesson content.</param>
    /// <param name="lessonData">The lesson contented as stored in its save file.</param>
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

    /// <summary>
    /// A method to test if the new body has been created.
    /// </summary>
    /// <param name="bodies">The bodies within the simulation.</param>
    /// <param name="newBody">The new body that has been created, used for comparison.</param>
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

    /// <summary>
    /// A method to test if a body has been edited.
    /// </summary>
    /// <param name="bodies">The bodies within the simulation.</param>
    /// <param name="editedBody">The body that has been edited, used for comparison.</param>
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

    /// <summary>
    /// A method to test if a body has been deleted.
    /// </summary>
    /// <param name="bodies">The bodies within the simulation.</param>
    /// <param name="deletedBody">The body that has been deleted, used for comparison.</param>
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

    /// <summary>
    /// A helper method used for the simulation save test, with it checking if the length instantiated data matches the saved data.
    /// </summary>
    /// <param name="instanceData">The instantiated SimulationSaveData object.</param>
    /// <param name="saveData">The data as read directly from the save file.</param>
    /// <returns>A Boolean value based on if the instance data and save data are identical.</returns>
    private static bool CompareSaveData(SimulationSaveData instanceData, SimulationSaveData saveData)
    {
        var bodies = instanceData.Bodies.Count == saveData.Bodies.Count;
        var pages = instanceData.LessonPages.Count == saveData.LessonPages.Count;
            
        return bodies && pages;
    }

    /// <summary>
    /// A method to test if a simulation has been correctly saved.
    /// </summary>
    /// <param name="filePath">The path where the save file is located.</param>
    /// <param name="instanceData">The currently instantiated simulation data.</param>
    /// <param name="saveSystem">A reference to a SaveSystem class.</param>
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

    /// <summary>
    /// A method to save the results of testing to a .txt file, only called when _testingComplete is set to true.
    /// </summary>
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