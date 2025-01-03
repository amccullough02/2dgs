using System;
using System.IO;
using System.Linq;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationMenu : GameState
{
    private Desktop _desktop;
    private FontSystem _fontSystem;
    private Game game;
    
    public SimulationMenu(Game game)
    {
        this.game = game;
        
        _fontSystem = new FontSystem();
        _fontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_light.ttf"));

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
            Font = _fontSystem.GetFont(80),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 50, 0, 0),
        };
        grid.Widgets.Add(title);
        Grid.SetRow(title, 0);
        
        // TABBED LISTS

        var tabControl = new TabControl {};
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
                Font = _fontSystem.GetFont(20)
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
                var button = new Button { Content = new Label { Text = fileName } };
                listView.Widgets.Add(button);
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