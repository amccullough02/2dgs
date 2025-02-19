using System;
using System.Collections.Generic;
using System.IO;

namespace _2dgs.Test;

public static class TestRunner
{
    private static List<string> _results = [];
    private static bool _testingComplete;
    private const string ResultsPath = "../../../savedata/test/results.txt";

    public static void AssertApplicationName(string actualName, string expectedName)
    {
        if (actualName == expectedName)
        {
            const string result = "TEST #1: Application name is as expected (PASS).";
            Console.WriteLine(result);
            _results.Add(result);
            _testingComplete = true;
        }
        else
        {
            const string result = "TEST #1: Application name is not as expected (FAIL).";
            Console.WriteLine(result);
            _results.Add(result);
            _testingComplete = true;
        }   
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
                writer.WriteLine(result + "\n");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _testingComplete = false;
    }
}