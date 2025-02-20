using System;
using System.IO;
using System.Linq;
using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;

namespace _2dgs;

public class SimulationMenuUi
{
    private readonly Desktop _desktop;
    private readonly FileManager _fileManager;

    public SimulationMenuUi(Game game)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        var simMenuTitle = UiComponents.TitleLabel("Simulation Menu");
        
        rootContainer.Widgets.Add(simMenuTitle);
        rootContainer.Widgets.Add(CreateSimulationMenu(game));
        
        _fileManager = new FileManager();
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel CreateSimulationMenu(Game game)
    {
        var verticalStackPanel = new VerticalStackPanel
        {
            Spacing = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 40, 0, 0),
        };

        var currentDirectory = Directory.GetCurrentDirectory();
        var saveDataDirectory = Path.Combine(currentDirectory, "../../../savedata/my_simulations");
        
        var importSimulationDialog = new FileDialog(FileDialogMode.OpenFile)
        {
            Filter = "*.json",
            Folder = saveDataDirectory,
        };

        importSimulationDialog.ButtonOk.Click += (_, _) =>
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
        importSimulationButton.Click += (_, _) =>
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
        exportSimulationButton.Click += (_, _) =>
        {
            exportSimulationDialog.Show(_desktop);
        };
        
        var mainMenuButton = UiComponents.Button("Return to Main Menu");
        mainMenuButton.Click += (_, _) => { game.SceneManager.PushScene(new FadeInScene(game, new MainMenuScene(game))); };

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

        var buttonPanel = new HorizontalStackPanel { HorizontalAlignment = HorizontalAlignment.Center, Spacing = 265 };
        buttonPanel.Widgets.Add(mainMenuButton);
        buttonPanel.Widgets.Add(importSimulationButton);
        buttonPanel.Widgets.Add(exportSimulationButton);
        
        verticalStackPanel.Widgets.Add(tabControl);
        verticalStackPanel.Widgets.Add(buttonPanel);
        
        TestRunner.AssertLessonsLoaded(lessonsListView, "../../../savedata/lessons");
        TestRunner.AssertUserSimsLoaded(sandboxListView, "../../../savedata/my_simulations");

        return verticalStackPanel;
    }
    
    private Dialog RenameButtonDialog(Game game, string fileName, string path, string file)
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
                    
        renameButtonDialog.ButtonOk.Click += (_, _) =>
        {
            Console.WriteLine($"DEBUG: {fileName} renamed to {textbox.Text}");
            var newPath = path + "/" + textbox.Text + ".json";
            _fileManager.RenameFile(file, newPath);
            game.SceneManager.ChangeScene(new SimulationMenuScene(game));
        };

        renameButtonDialog.ButtonCancel.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: File rename cancelled");
        };
                    
        return renameButtonDialog;
    }
    
    private Dialog DeleteButtonDialog(Game game, string fileName, string path)
    {
        var deleteButtonDialog = UiComponents.StyledDialog("Delete");
        deleteButtonDialog.Content = UiComponents.LightLabel("Are you sure you want to delete this simulation?");

        deleteButtonDialog.ButtonOk.Click += (_, _) =>
        {
            Console.WriteLine($"DEBUG: {fileName} deleted");
            _fileManager.DeleteFile(path + "/" + fileName + ".json");
            game.SceneManager.ChangeScene(new SimulationMenuScene(game));
        };

        deleteButtonDialog.ButtonCancel.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Delete operation cancelled");
        };
                    
        return deleteButtonDialog;
    }
    
    private HorizontalStackPanel CreateFilePanel(Game game, string file, string path, string description, string thumbnail)
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

        Image LoadThumbnailImage(GraphicsDevice graphicsDevice, string thumbnailPath)
        {
            using var stream = File.OpenRead(thumbnailPath);
            var texture = Texture2D.FromStream(graphicsDevice, stream);
            var image = new Image
            {
                Renderable = new TextureRegion(texture), 
                BorderThickness = new Thickness(1), 
                Border = new SolidBrush(Color.Gray)
            };
            return image;
        }
        
        var thumbnailContainer = new Panel
        {
            Width = 192,
            Height = 128,
        };
        
        thumbnailContainer.Widgets.Add(LoadThumbnailImage(game.GraphicsDevice, thumbnail));
        
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
        thumbnailPanel.Widgets.Add(thumbnailContainer);
        
        var descriptionTextBox = UiComponents.ReadOnlyTextBox(description);
        descriptionTextBox.Width = 790;
        
        var loadButton = UiComponents.Button("Load", true, 200, 50);
        var renameButton = UiComponents.Button("Rename", true, 200, 50);
        var renameDialog = RenameButtonDialog(game, fileName, path, file);
        var deleteButton = UiComponents.Button("Delete", true, 200, 50);
        var deleteDialog = DeleteButtonDialog(game, fileName, path);

        loadButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation...");
            game.SceneManager.PushScene(new FadeInScene(game, new SimulationScene(game, file)));
        };

        renameButton.Click += (_, _) =>
        {
            renameDialog.Show(_desktop);
        };
        
        deleteButton.Click += (_, _) =>
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
            Padding = new Thickness(UiConstants.DefaultLargePadding),
            Spacing = 10,
            Background = new SolidBrush(new Color(24, 24, 24))
        };
        
        fileStackPanel.Widgets.Add(thumbnailPanel);
        fileStackPanel.Widgets.Add(descriptionTextBox);
        fileStackPanel.Widgets.Add(optionsStackPanel);
        
        return fileStackPanel;
    }

    private string GetSimulationDescription(Game game, string path)
    {
        var data = game.SaveSystem.LoadSimulation(path);
        return string.IsNullOrEmpty(data.Description) ? "No description found." : data.Description;
    }

    private string GetSimulationThumbnailPath(Game game, string path)
    {
        const string defaultPath = "../../../savedata/thumbnails/default.png";
        var data = game.SaveSystem.LoadSimulation(path);
        return string.IsNullOrEmpty(data.ThumbnailPath) ? defaultPath : data.ThumbnailPath;
    }
    
    private void PopulateList(ListView listView, string path, Game game)
    {
        if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
        {
            var files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
            {
                listView.Widgets.Add(CreateFilePanel(game, file, path, GetSimulationDescription(game, file),
                    GetSimulationThumbnailPath(game, file)));
            }
        }
        else
        {
            var noFilesLabel = UiComponents.MediumLabel("No files found.");
            noFilesLabel.Padding = new Thickness(UiConstants.DefaultMediumPadding);
            listView.Widgets.Add(noFilesLabel);
        }
    }

    public void Draw()
    {
        _desktop.Render();
    }
}