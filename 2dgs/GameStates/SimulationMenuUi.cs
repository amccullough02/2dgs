using System;
using System.IO;
using System.Linq;
using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationMenuUi
{
    private Desktop _desktop;
    private FileManager _fileManager;

    public SimulationMenuUi(Game game)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(CreateSimulationMenu(game));
        
        _fileManager = new FileManager();
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private Grid CreateSimulationMenu(Game game)
    {
        var grid = UiComponents.CreateGrid(10);
        UiComponents.AddGridColumns(grid, 1);
        UiComponents.AddGridRows(grid, 3);

        var title = UiComponents.CreateStyledLabel("Simulation Menu");
        Grid.SetRow(title, 0);
        
        var tabControl = new TabControl
        {
            Width = 400,
            MouseCursor = MouseCursorType.Hand,
        };
        Grid.SetRow(tabControl, 1);
        
        var lessonsListView = new ListView();
        var userSimulationsListView = new ListView();

        var lessonsTab = new TabItem
        {
            Text = "Lessons",
            Content = lessonsListView,
        };
        
        var userSimsTab = new TabItem
        {
            Text = "My Simulations",
            Content = userSimulationsListView
        };
        
        tabControl.Items.Add(lessonsTab);
        tabControl.Items.Add(userSimsTab);
        
        PopulateList(lessonsListView, "../../../sims/lessons", game);
        PopulateList(userSimulationsListView, "../../../sims/my_simulations", game);
        TestFileLoading(lessonsListView);
        
        var returnButton = UiComponents.CreateButton("Main Menu");
        returnButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to main menu...");
            game.GameStateManager.ChangeState(new MainMenu(game));
        };
        Grid.SetRow(returnButton, 2);
        
        grid.Widgets.Add(title);
        grid.Widgets.Add(tabControl);
        grid.Widgets.Add(returnButton);

        return grid;
    }
    
    private void EditButtonDialog(string fileName, string path, string file)
    {
        var textbox = new TextBox { Text = fileName };
        
        var popup = new Dialog
        {
            Title = "Rename Simulation",
            Content = textbox,
        };
                    
        popup.ButtonOk.Click += (sender, result) =>
        {
            Console.WriteLine($"DEBUG: {fileName} renamed to {textbox.Text}");
            var newPath = path + "/" + textbox.Text + ".json";
            _fileManager.RenameFile(file, newPath);
        };

        popup.ButtonCancel.Click += (sender, result) =>
        {
            Console.WriteLine("DEBUG: File rename cancelled");
        };
                    
        popup.Show(_desktop);
    }
    
    private void DeleteButtonDialog(string fileName, string path)
    {
        var popup = new Dialog
        {
            Title = "Delete Simulation"
        };

        popup.ButtonOk.Click += (sender, result) =>
        {
            Console.WriteLine($"DEBUG: {fileName} deleted");
            _fileManager.DeleteFile(path + "/" + fileName + ".json");
        };

        popup.ButtonCancel.Click += (sender, result) =>
        {
            Console.WriteLine("DEBUG: Delete operation cancelled");
        };
                    
        popup.Show(_desktop);
    }
    
    private HorizontalStackPanel CreateFilePanel(string file, string path, Game game)
    {
        var fileName = Path.GetFileNameWithoutExtension(file);
        var loadButton = new Button { Content = new Label { Text = fileName } };
        var editButton = new Button { Content = new Label { Text = "Edit"}};
        var deleteButton = new Button { Content = new Label { Text = "Delete"}};

        loadButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation...");
            game.GameStateManager.ChangeState(new Simulation(game, file));
        };

        editButton.Click += (s, a) =>
        {
            EditButtonDialog(fileName, path, file);
        };
        
        deleteButton.Click += (s, a) =>
        {
            DeleteButtonDialog(fileName, path);
        };
        
        var fileStackPanel = new HorizontalStackPanel { Spacing = 15, };
        fileStackPanel.Widgets.Add(loadButton);
        fileStackPanel.Widgets.Add(editButton);
        fileStackPanel.Widgets.Add(deleteButton);
        
        return fileStackPanel;
    }
    
    private void PopulateList(ListView listView, string path, Game game)
    {
        if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
        {
            var files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
            {
                Console.WriteLine(file);
                listView.Widgets.Add(CreateFilePanel(file, path, game));
            }
        }
        else
        {
            var noFilesLabel = new Label { Text = "No files found" };
            listView.Widgets.Add(noFilesLabel);
        }
    }
    
    private void TestFileLoading(ListView listView)
    {
        if (listView.Widgets.Count == Directory.EnumerateFileSystemEntries("../../../sims/lessons").Count())
        {
            Console.WriteLine("TEST - Lessons files loaded... PASS!");
        }
        else
        {
            Console.WriteLine("TEST - Lessons files loaded... FAIL!");
        }
    }

    public void Draw()
    {
        _desktop.Render();
    }
}