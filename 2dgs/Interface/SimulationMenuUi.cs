using System;
using System.IO;
using System.Linq;
using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;

namespace _2dgs;

public class SimulationMenuUi
{
    private readonly Desktop _desktop;
    private readonly FileManager _fileManager;

    private static readonly string DescriptionPlaceholder =
        "Description: In physics, specifically classical mechanics, the three-body problem is to take the initial positions and " +
        "velocities (or momenta) of three point masses that orbit each other in space and calculate their subsequent " +
        "trajectories using Newton's laws of motion and Newton's law of universal gravitation.";

    public SimulationMenuUi(Game game, SaveSystem saveSystem)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        var simMenuTitle = UiComponents.TitleLabel("Simulation Menu");
        
        rootContainer.Widgets.Add(simMenuTitle);
        rootContainer.Widgets.Add(CreateSimulationMenu(game, saveSystem));
        
        _fileManager = new FileManager();
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel CreateSimulationMenu(Game game, SaveSystem saveSystem)
    {
        var verticalStackPanel = UiComponents.VerticalStackPanel(20, HorizontalAlignment.Center, 
            VerticalAlignment.Center, new Thickness(0, 40, 0, 0));
        
        var createNewSimulationButton = UiComponents.Button("Create New Simulation");
        
        var createNewSimulationDialog = NameSimulationDialog(game, saveSystem);
        
        createNewSimulationButton.Click += (s, e) =>
        {
            createNewSimulationDialog.Show(_desktop);
        };

        var currentDirectory = Directory.GetCurrentDirectory();
        var saveDataDirectory = Path.Combine(currentDirectory, "../../../savedata/my_simulations");
        
        var importSimulationDialog = new FileDialog(FileDialogMode.OpenFile)
        {
            Filter = "*.json",
            Folder = saveDataDirectory,
        };

        importSimulationDialog.ButtonOk.Click += (s, e) =>
        {
            var selectedFilePath = importSimulationDialog.FilePath;

            if (selectedFilePath.Length == 0) return;

            var destinationFilePath = Path.Combine(saveDataDirectory, Path.GetFileName(selectedFilePath));

            try
            {
                File.Copy(importSimulationDialog.FilePath, destinationFilePath, true);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception);
            }
            
        };

        var importSimulationButton = UiComponents.Button("Import Simulation");
        importSimulationButton.Click += (s, e) =>
        {
            importSimulationDialog.Show(_desktop);
        };
        
        var exportSimulationDialog = new FileDialog(FileDialogMode.OpenFile)
        {
            Filter = "*.json",
            Folder = saveDataDirectory,
        };

        exportSimulationDialog.ButtonOk.Click += (_, _) =>
        {
            var selectedFilePath = exportSimulationDialog.FilePath;
            Console.WriteLine(selectedFilePath);

            if (selectedFilePath.Length == 0) return;

            var folderSelectionDialog = new FileDialog(FileDialogMode.ChooseFolder)
            {
                Folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            folderSelectionDialog.ButtonOk.Click += (_, _) =>
            {
                if (string.IsNullOrEmpty(folderSelectionDialog.FilePath)) return;
                
                string destinationFilePath = Path.Combine(folderSelectionDialog.Folder, Path.GetFileName(selectedFilePath));
                
                Console.WriteLine(destinationFilePath);

                try
                {
                    File.Copy(selectedFilePath, destinationFilePath, true);
                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception);
                }
            };
            
            folderSelectionDialog.Show(_desktop);
        };
        
        var exportSimulationButton = UiComponents.Button("Export Simulation");
        exportSimulationButton.Click += (s, e) =>
        {
            exportSimulationDialog.Show(_desktop);
        };
        
        var mainMenuButton = UiComponents.Button("Return to Main Menu");
        mainMenuButton.Click += (s, e) => { game.GameStateManager.ChangeState(new MainMenu(game)); };

        RichTextDefaults.FontResolver = p =>
        {
            var args = p.Split(',');
            var fontName = args[0].Trim();
            var fontSize = int.Parse(args[1].Trim());
            var fontPath = "../../../Content/fonts/" + fontName + ".ttf";

            var fontSystem = new FontSystem();
            fontSystem.AddFont(File.ReadAllBytes(fontPath));
            return fontSystem.GetFont(fontSize);
        };
        
        var tabControl = UiComponents.TabControl(1280, 720);
        var lessonsTab = UiComponents.TabItem("/f[LeagueSpartan-Medium,20]\nLesson Simulations\n");
        var lessonsListView = UiComponents.ListView(1280);
        lessonsTab.Content = lessonsListView;
        
        var userSimsTab = UiComponents.TabItem("/f[LeagueSpartan-Medium,20]\nSandbox Simulations\n");
        var sandboxListView = UiComponents.ListView(1280);
        userSimsTab.Content = sandboxListView;
        
        tabControl.Items.Add(lessonsTab);
        tabControl.Items.Add(userSimsTab);
        
        PopulateList(lessonsListView, "../../../savedata/lessons", game);
        PopulateList(sandboxListView, "../../../savedata/my_simulations", game);

        var buttonPanel = new HorizontalStackPanel { HorizontalAlignment = HorizontalAlignment.Center, Spacing = 90 };
        buttonPanel.Widgets.Add(mainMenuButton);
        buttonPanel.Widgets.Add(importSimulationButton);
        buttonPanel.Widgets.Add(exportSimulationButton);
        buttonPanel.Widgets.Add(createNewSimulationButton);
        
        verticalStackPanel.Widgets.Add(tabControl);
        verticalStackPanel.Widgets.Add(buttonPanel);
        
        UiTests.TestSimFileLoading(lessonsListView, "../../../savedata/lessons");

        return verticalStackPanel;
    }
    
    private Dialog NameSimulationDialog(Game game, SaveSystem saveSystem)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 1);
        var nameSimulationLabel = UiComponents.MediumLabel("Name simulation: ");
        Grid.SetColumn(nameSimulationLabel, 0);

        var nameSimulationTextbox = UiComponents.TextBox("new_simulation");
        Grid.SetColumn(nameSimulationTextbox, 1);
        
        grid.Widgets.Add(nameSimulationLabel);
        grid.Widgets.Add(nameSimulationTextbox);
        
        var newSimulationDialog = UiComponents.StyledDialog("New simulation");
        newSimulationDialog.Content = grid;
        newSimulationDialog.ButtonOk.Click += (sender, e) =>
        {
            var newFilePath = "../../../savedata/my_simulations/" + nameSimulationTextbox.Text + ".json";
            saveSystem.CreateBlankSimulation(newFilePath);
            game.GameStateManager.ChangeState(new Simulation(game, newFilePath));
        };
        
        return newSimulationDialog;
    }
    
    private Dialog RenameButtonDialog(string fileName, string path, string file)
    {
        var renameButtonDialog = UiComponents.StyledDialog("Rename");
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 1);
        
        var label = UiComponents.MediumLabel("New file name: ");
        Grid.SetColumn(label, 0);
        var textbox = UiComponents.TextBox(fileName);
        Grid.SetColumn(textbox, 1);
        
        grid.Widgets.Add(label);
        grid.Widgets.Add(textbox);
        
        renameButtonDialog.Content = grid;
                    
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
                    
        return renameButtonDialog;
    }
    
    private Dialog DeleteButtonDialog(string fileName, string path)
    {
        var deleteButtonDialog = UiComponents.StyledDialog("Delete");
        deleteButtonDialog.Content = UiComponents.LightLabel("Are you sure you want to delete this simulation?");

        deleteButtonDialog.ButtonOk.Click += (sender, result) =>
        {
            Console.WriteLine($"DEBUG: {fileName} deleted");
            _fileManager.DeleteFile(path + "/" + fileName + ".json");
        };

        deleteButtonDialog.ButtonCancel.Click += (sender, result) =>
        {
            Console.WriteLine("DEBUG: Delete operation cancelled");
        };
                    
        return deleteButtonDialog;
    }
    
    private HorizontalStackPanel CreateFilePanel(string file, string path, Game game)
    {
        var fileName = Path.GetFileNameWithoutExtension(file);
        
        var simulationLabel = UiComponents.MediumLabel(StringTransformer.FileNamePrettier(fileName));
        simulationLabel.HorizontalAlignment = HorizontalAlignment.Center;
        simulationLabel.BorderThickness = new Thickness(1);

        var line = new HorizontalSeparator
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            Thickness = 1,
            Color = Color.White,
            Width = 128
        };

        var thumbnailPlaceholderLabel = UiComponents.LightLabel("Fancy Thumbnail!");
        thumbnailPlaceholderLabel.HorizontalAlignment = HorizontalAlignment.Center;
        thumbnailPlaceholderLabel.VerticalAlignment = VerticalAlignment.Center;
        
        var thumbnailPlaceholder = new Panel
        {
            Width = 192,
            Height = 128,
            Background = new SolidBrush(Color.Black),
            Padding = new Thickness(10),
        };
        thumbnailPlaceholder.Widgets.Add(thumbnailPlaceholderLabel);
        
        var thumbnailPanel = new VerticalStackPanel
        {
            Spacing = 4,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Center,
            BorderThickness = new Thickness(1),
            Border = new SolidBrush(Color.Gray),
            Padding = new Thickness(4),
            Width = 200,
        };
        
        thumbnailPanel.Widgets.Add(simulationLabel);
        thumbnailPanel.Widgets.Add(line);
        thumbnailPanel.Widgets.Add(thumbnailPlaceholder);
        
        var descriptionTextBox = UiComponents.ReadOnlyTextBox(DescriptionPlaceholder);
        descriptionTextBox.Width = 790;
        
        var loadButton = UiComponents.Button("Load", true, 200, 50);
        var renameButton = UiComponents.Button("Rename", true, 200, 50);
        var renameDialog = RenameButtonDialog(fileName, path, file);
        var deleteButton = UiComponents.Button("Delete", true, 200, 50);
        var deleteDialog = DeleteButtonDialog(fileName, path);

        loadButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation...");
            game.GameStateManager.ChangeState(new Simulation(game, file));
        };

        renameButton.Click += (s, a) =>
        {
            renameDialog.Show(_desktop);
        };
        
        deleteButton.Click += (s, a) =>
        {
            deleteDialog.Show(_desktop);
        };

        var optionsStackPanel = new VerticalStackPanel
        {
            Spacing = 10,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        
        optionsStackPanel.Widgets.Add(loadButton);
        optionsStackPanel.Widgets.Add(renameButton);
        optionsStackPanel.Widgets.Add(deleteButton);
        
        var fileStackPanel = new HorizontalStackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Padding = new Thickness(10),
            Spacing = 10,
            Background = new SolidBrush(new Color(24, 24, 24))
        };
        
        fileStackPanel.Widgets.Add(thumbnailPanel);
        fileStackPanel.Widgets.Add(descriptionTextBox);
        fileStackPanel.Widgets.Add(optionsStackPanel);
        
        return fileStackPanel;
    }
    
    private void PopulateList(ListView listView, string path, Game game)
    {
        if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
        {
            var files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
            {
                listView.Widgets.Add(CreateFilePanel(file, path, game));
            }
        }
        else
        {
            var noFilesLabel = new Label { Text = "No files found" };
            listView.Widgets.Add(noFilesLabel);
        }
    }

    public void Draw()
    {
        _desktop.Render();
    }
}