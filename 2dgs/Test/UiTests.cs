using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class UiTests
{
    public static void TestLessonPrompt(List<LessonPage> fileData, List<LessonPage> displayData)
    {
        if (fileData[0].Text == displayData[0].Text)
        {
            Console.WriteLine("UI Test - Lesson prompt... PASS!");
        }
        else
        {
            Console.WriteLine("UI Test - Lesson prompt... FAIL!");
        }
    }

    public static void TestSimFileLoading(ListView listView, string filePath)
    {
        if (listView.Widgets.Count == Directory.EnumerateFileSystemEntries(filePath).Count())
        {
            Console.WriteLine("UI TEST - Lessons files loaded... PASS!");
        }
        else
        {
            Console.WriteLine("UI TEST - Lessons files loaded... FAIL!");
        }
    }
}