using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationMenu : GameState
{
    private Desktop _desktop;
    private FontManager _fontManager;
    private FileManager _fileManager;
    private Game game;
    
    public SimulationMenu(Game game)
    {
        this.game = game;
        
        _fontManager = new FontManager();
        _fileManager = new FileManager();

        MyraEnvironment.Game = this.game;

        var grid = new Grid
        {
            RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        
        // COLUMNS
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        
        // ROWS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // TITLE
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // TABBED LIST
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // RETURN BUTTON
        
        var title = new Label
        {
            Id = "title",
            Text = "Simulation Menu",
            Font = _fontManager.GetOrbitronLightFont(70),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 50, 0, 0),
        };
        grid.Widgets.Add(title);
        Grid.SetRow(title, 0);
        
        // TABBED LISTS

        var tabControl = new TabControl
        {
            Width = 400,
            MouseCursor = MouseCursorType.Hand,
        };
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
        
        grid.Widgets.Add(tabControl);
        Grid.SetRow(tabControl, 1);
        
        PopulateList(lessonsListView, "../../../sims/lessons");
        PopulateList(userSimulationsListView, "../../../sims/my_simulations");

        if (lessonsListView.Widgets.Count == 
            Directory.EnumerateFileSystemEntries("../../../sims/lessons").Count())
        {
            Console.WriteLine("TEST - Lessons files loaded... PASS!");
        }
        else
        {
            Console.WriteLine("TEST - Lessons files loaded... FAIL!");
        }
        
        var returnButton = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Return to Main Menu",
                Font = _fontManager.GetOrbitronLightFont(20)
            }
        };
        Grid.SetRow(returnButton, 2);

        returnButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to main menu...");
            this.game.GameStateManager.ChangeState(new MainMenu(this.game));
        };

        grid.Widgets.Add(returnButton);
        
        _desktop = new Desktop();
        _desktop.Root = grid;
    }

    private void PopulateList(ListView listView, string path)
    {
        if (Directory.Exists(path) && Directory.EnumerateFileSystemEntries(path).Any())
        {
            var files = Directory.GetFiles(path, "*.json");
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var loadButton = new Button { Content = new Label { Text = fileName } };
                var editButton = new Button { Content = new Label { Text = "Edit"}};
                var deleteButton = new Button { Content = new Label { Text = "Delete"}};

                loadButton.Click += (s, a) =>
                {
                    game.GameStateManager.ChangeState(new Simulation(game, file));
                };

                editButton.Click += (s, a) =>
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
                };
                
                deleteButton.Click += (s, a) =>
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
                };
                
                var fileStackPanel = new HorizontalStackPanel { Spacing = 15, };
                fileStackPanel.Widgets.Add(loadButton);
                fileStackPanel.Widgets.Add(editButton);
                fileStackPanel.Widgets.Add(deleteButton);
                listView.Widgets.Add(fileStackPanel);
            }
        }
        else
        {
            var noFilesLabel = new Label { Text = "No files found" };
            listView.Widgets.Add(noFilesLabel);
        }
    }
    
    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _desktop.Render();
    }
}