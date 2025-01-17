using System;

namespace _2dgs;

public static class UiTests
{
    public static void TestLessonPrompt(string[] fileData, string[] displayData)
    {
        if (fileData[0] == displayData[0])
        {
            Console.WriteLine("Test - Lesson prompt... PASS!");
        }
        else
        {
            Console.WriteLine("Test - Lesson prompt... FAIL!");
        }
    }
}