using System;
using System.IO;

namespace _2dgs;

/// <summary>
/// A class used to perform rename and delete operations on files, making use of C#'s standard library.
/// </summary>
public class FileManager
{
    /// <summary>
    /// Renames a file.
    /// </summary>
    /// <param name="oldPath">The current path of the file to be renamed.</param>
    /// <param name="newPath">The new path of the file to be renamed.</param>
    public void RenameFile(string oldPath, string newPath)
    {
        try
        {
            if (File.Exists(oldPath))
            {
                if (!File.Exists(newPath))
                {
                    File.Move(oldPath, newPath);
                    Console.WriteLine($"DEBUG: File {oldPath} renamed to {newPath}");
                }
                else
                {
                    Console.WriteLine($"DEBUG: File {newPath} already exists");
                }
            }
            else
            {
                Console.WriteLine($"DEBUG: File {oldPath} does not exist");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Deletes a file.
    /// </summary>
    /// <param name="filePath">The path of the file to be deleted.</param>
    public void DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine($"DEBUG: File {filePath} deleted successfully");
            }
            else
            {
                Console.WriteLine($"DEBUG: File {filePath} not found");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}