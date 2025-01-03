using System;
using System.IO;

namespace _2dgs;

public class FileManager
{
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

    public void DeleteFile(string file)
    {
        try
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                Console.WriteLine($"DEBUG: File {file} deleted successfully");
            }
            else
            {
                Console.WriteLine($"DEBUG: File {file} not found");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}