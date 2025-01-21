using System;
using System.Collections.Generic;

namespace _2dgs;

public static class UiTests
{
    public static void TestLessonPrompt(List<LessonPage> fileData, List<LessonPage> displayData)
    {
        if (fileData[0].Text == displayData[0].Text)
        {
            Console.WriteLine("Test - Lesson prompt... PASS!");
        }
        else
        {
            Console.WriteLine("Test - Lesson prompt... FAIL!");
        }
    }
}