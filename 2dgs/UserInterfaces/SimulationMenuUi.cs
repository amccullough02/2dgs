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

/// <summary>
/// A class used to contain UI boilerplate for the SimulationMenuUi Scene.
/// </summary>
public class SimulationMenuUi
{
    /// <summary>
    /// An instance of a Myra Desktop, the highest unit of organisation in Myra's UI system.
    /// </summary>
    private readonly Desktop _desktop;

    /// <summary>
    /// The constructor for the SimulationMenuUi class.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    public SimulationMenuUi(Game game)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        var simMenuTitle = UiComponents.TitleLabel("Simulation Menu");
        
        rootContainer.Widgets.Add(simMenuTitle);
        rootContainer.Widgets.Add(CreateSimulationMenu(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    /// <summary>
    /// Returns a vertical stack panel that organises the simulation menu UI components.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <returns>A vertical stack panel with the organised UI components.</returns>
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
    
    /// <summary>
    /// A method used to create a dialog that allows simulations to be renamed.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="fileName">The original simulation file name which is used as the text content of the dialog's textbox.</param>
    /// <param name="folderPath">The folder path in which the simulation file is stored.</param>
    /// <param name="originalFilePath">The original simulation file path in its entirety.</param>
    /// <returns>A Myra Dialog class with simulation renaming logic.</returns>
    private Dialog RenameButtonDialog(Game game, string fileName, string folderPath, string originalFilePath)
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
            var newPath = folderPath + "/" + textbox.Text + ".json";
            FileManager.RenameFile(originalFilePath, newPath);
            TestRunner.AssertFileRename(originalFilePath, newPath);
            game.SceneManager.ChangeScene(new SimulationMenuScene(game));
        };

        renameButtonDialog.ButtonCancel.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: File rename cancelled");
        };
                    
        return renameButtonDialog;
    }
    
    /// <summary>
    /// A method used to create a dialog that allows simulations to be deleted.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="fileName">The original simulation file name.</param>
    /// <param name="folderPath">The folder path in which the simulation file is stored.</param>
    /// <returns>A Myra Dialog class with simulation deletion logic.</returns>
    private Dialog DeleteButtonDialog(Game game, string fileName, string folderPath)
    {
        var deleteButtonDialog = UiComponents.StyledDialog("Delete");
        deleteButtonDialog.Content = UiComponents.LightLabel("Are you sure you want to delete this simulation?");

        deleteButtonDialog.ButtonOk.Click += (_, _) =>
        {
            Console.WriteLine($"DEBUG: {fileName} deleted");
            FileManager.DeleteFile(folderPath + "/" + fileName + ".json");
            TestRunner.AssertFileDeletion(folderPath + "/" + fileName + ".json");
            game.SceneManager.ChangeScene(new SimulationMenuScene(game));
        };

        deleteButtonDialog.ButtonCancel.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Delete operation cancelled");
        };
                    
        return deleteButtonDialog;
    }
    
    /// <summary>
    /// A method used to create a 'file panel' a list element that contains simulation metadata and rename/delete options.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="filePath">The direct path to the simulation file.</param>
    /// <param name="folderPath">The path to the simulation folder the file is within.</param>
    /// <param name="description">The description of the simulation.</param>
    /// <param name="thumbnail">The thumbnail path for the simulation.</param>
    /// <returns>A horizontal stack panel containing simulation metadata and options, which is to be added to a listview.</returns>
    private HorizontalStackPanel CreateFilePanel(Game game, string filePath, string folderPath, string description, string thumbnail)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        
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
        var renameDialog = RenameButtonDialog(game, fileName, folderPath, filePath);
        var deleteButton = UiComponents.Button("Delete", true, 200, 50);
        var deleteDialog = DeleteButtonDialog(game, fileName, folderPath);

        loadButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation...");
            game.SceneManager.PushScene(new FadeInScene(game, new SimulationScene(game, filePath)));
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

    /// <summary>
    /// A method used to obtain the text description of a simulation.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="filePath">The direct path to the simulation file.</param>
    /// <returns>A string with the simulation's description.</returns>
    private string GetSimulationDescription(Game game, string filePath)
    {
        var data = game.SaveSystem.LoadSimulation(filePath);
        return string.IsNullOrEmpty(data.Description) ? "No description found." : data.Description;
    }

    /// <summary>
    /// A method used to obtain the thumbnail path of a simulation.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="filePath">The direct path to the simulation file.</param>
    /// <returns>A string containing the thumbnail path for the simulation.</returns>
    private string GetSimulationThumbnailPath(Game game, string filePath)
    {
        const string defaultPath = "../../../savedata/thumbnails/default.png";
        var data = game.SaveSystem.LoadSimulation(filePath);
        return string.IsNullOrEmpty(data.ThumbnailPath) ? defaultPath : data.ThumbnailPath;
    }
    
    /// <summary>
    /// Populates a Myra ListView with list elements that contain the simulation name, thumbnail, description, and file manipulation options.
    /// </summary>
    /// <param name="listView">The ListView widget to append the list elements to.</param>
    /// <param name="folderPath">The simulation folder path.</param>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    private void PopulateList(ListView listView, string folderPath, Game game)
    {
        if (Directory.Exists(folderPath) && Directory.EnumerateFileSystemEntries(folderPath).Any())
        {
            var files = Directory.GetFiles(folderPath, "*.json");
            foreach (var file in files)
            {
                listView.Widgets.Add(CreateFilePanel(game, file, folderPath, GetSimulationDescription(game, file),
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

    /// <summary>
    /// Draws the Myra desktop.
    /// </summary>
    public void Draw()
    {
        _desktop.Render();
    }
}