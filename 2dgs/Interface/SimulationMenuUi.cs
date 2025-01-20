using System;
using System.IO;
using System.Linq;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationMenuUi
{
    private readonly Desktop _desktop;
    private readonly FileManager _fileManager;

    public SimulationMenuUi(Game game, SaveSystem saveSystem)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        var simMenuTitle = UiComponents.TitleLabel("Simulation Menu");
        
        rootContainer.Widgets.Add(simMenuTitle);
        rootContainer.Widgets.Add(CreateSimulationMenu(game, saveSystem));
        rootContainer.Widgets.Add(ExitPanel(game));
        
        _fileManager = new FileManager();
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel CreateSimulationMenu(Game game, SaveSystem saveSystem)
    {
        var verticalStackPanel = UiComponents.VerticalStackPanel(8,
            HorizontalAlignment.Center,
            VerticalAlignment.Center,
            new Thickness(UiConstants.DefaultMargin));
        
        var createNewSimulationButton = UiComponents.Button("Create New Simulation");
        
        var createNewSimulationDialog = NameSimulationDialog(game, saveSystem);
        
        createNewSimulationButton.Click += (s, e) =>
        {
            createNewSimulationDialog.Show(_desktop);
        };

        var tabControl = UiComponents.TabControl(400);

        var lessonsTab = UiComponents.TabItem("Lesson Simulations");
        var lessonsListView = UiComponents.ListView(400);
        lessonsTab.Content = lessonsListView;
        
        var userSimsTab = UiComponents.TabItem("Sandbox Simulations");
        var userSimulationsListView = UiComponents.ListView(400);
        userSimsTab.Content = userSimulationsListView;
        
        tabControl.Items.Add(lessonsTab);
        tabControl.Items.Add(userSimsTab);
        
        PopulateList(lessonsListView, "../../../sims/lessons", game);
        PopulateList(userSimulationsListView, "../../../sims/my_simulations", game);
        TestFileLoading(lessonsListView);
        
        userSimulationsListView.Widgets.Add(createNewSimulationButton);
        verticalStackPanel.Widgets.Add(tabControl);

        return verticalStackPanel;
    }
    
    private Dialog NameSimulationDialog(Game game, SaveSystem saveSystem)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 1);
        var nameSimulationLabel = UiComponents.DialogLabel("Name simulation: ");
        Grid.SetColumn(nameSimulationLabel, 0);

        var nameSimulationTextbox = UiComponents.TextBox("new_simulation");
        Grid.SetColumn(nameSimulationTextbox, 1);
        
        grid.Widgets.Add(nameSimulationLabel);
        grid.Widgets.Add(nameSimulationTextbox);
        
        var newSimulationDialog = UiComponents.StyledDialog("New simulation");
        newSimulationDialog.Content = grid;
        newSimulationDialog.ButtonOk.Click += (sender, e) =>
        {
            string newFilePath = "../../../sims/my_simulations/" + nameSimulationTextbox.Text + ".json";
            saveSystem.CreateBlankSimulation(newFilePath);
            game.GameStateManager.ChangeState(new Simulation(game, newFilePath));
        };
        
        return newSimulationDialog;
    }
    
    private void RenameButtonDialog(string fileName, string path, string file)
    {
        var textbox = UiComponents.TextBox(fileName);
        
        var renameButtonDialog = UiComponents.StyledDialog("Rename");
        renameButtonDialog.Content = textbox;
                    
        renameButtonDialog.ButtonOk.Click += (sender, result) =>
        {
            Console.WriteLine($"DEBUG: {fileName} renamed to {textbox.Text}");
            var newPath = path + "/" + textbox.Text + ".json";
            _fileManager.RenameFile(file, newPath);
        };

        renameButtonDialog.ButtonCancel.Click += (sender, result) =>
        {
            Console.WriteLine("DEBUG: File rename cancelled");
        };
                    
        renameButtonDialog.Show(_desktop);
    }
    
    private void DeleteButtonDialog(string fileName, string path)
    {
        var deleteButtonDialog = UiComponents.StyledDialog("Delete");

        deleteButtonDialog.ButtonOk.Click += (sender, result) =>
        {
            Console.WriteLine($"DEBUG: {fileName} deleted");
            _fileManager.DeleteFile(path + "/" + fileName + ".json");
        };

        deleteButtonDialog.ButtonCancel.Click += (sender, result) =>
        {
            Console.WriteLine("DEBUG: Delete operation cancelled");
        };
                    
        deleteButtonDialog.Show(_desktop);
    }
    
    private HorizontalStackPanel CreateFilePanel(string file, string path, Game game)
    {
        var fileName = Path.GetFileNameWithoutExtension(file);
        var loadButton = new Button { Content = UiComponents.Label(fileName) };
        var editButton = new Button { Content = UiComponents.Label("Rename")};
        var deleteButton = new Button { Content = UiComponents.Label("Delete")};

        loadButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation...");
            game.GameStateManager.ChangeState(new Simulation(game, file));
        };

        editButton.Click += (s, a) =>
        {
            RenameButtonDialog(fileName, path, file);
        };
        
        deleteButton.Click += (s, a) =>
        {
            DeleteButtonDialog(fileName, path);
        };
        
        var fileStackPanel = new HorizontalStackPanel { Spacing = 15, Margin = new Thickness(10), VerticalAlignment = VerticalAlignment.Center };
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
    
    private VerticalStackPanel ExitPanel(Game game)
    {
        var button = UiComponents.Button("Return to Main Menu");
        button.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new MainMenu(game));
        };

        var verticalStackPanel = UiComponents.VerticalStackPanel(8, HorizontalAlignment.Left, 
            VerticalAlignment.Bottom, new Thickness(UiConstants.DefaultMargin));
        
        verticalStackPanel.Widgets.Add(button);
        
        return verticalStackPanel;
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