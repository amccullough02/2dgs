using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class MainMenu : GameState
{
    private Desktop _desktop;
    private FontManager _fontManager;
    private Game game;

    public MainMenu(Game game)
    {
        this.game = game;
        
        _fontManager = new FontManager();

        MyraEnvironment.Game = this.game;

        var grid = new Grid
        {
            RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        // COLUMNS
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        
        // ROWS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // TITLE
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // SIMS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // SETTINGS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // QUIT

        var title = new Label
        {
            Id = "title",
            Text = "2DGS",
            Font = _fontManager.GetOrbitronLightFont(70),
            HorizontalAlignment = HorizontalAlignment.Center
        };
        Grid.SetRow(title, 0);
        grid.Widgets.Add(title);
        
        var simulationMenu = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Simulations",
                Font = _fontManager.GetOrbitronLightFont(20),
            }
        };
        Grid.SetRow(simulationMenu, 1);

        simulationMenu.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation menu...");
            this.game.GameStateManager.ChangeState(new SimulationMenu(this.game));
        };

        grid.Widgets.Add(simulationMenu);
        
        var settingsMenu = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Settings",
                Font = _fontManager.GetOrbitronLightFont(20),
            }
        };
        Grid.SetRow(settingsMenu, 2);

        settingsMenu.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to settings menu...");
            this.game.GameStateManager.ChangeState(new SettingsMenu(this.game));
        };

        grid.Widgets.Add(settingsMenu);
        
        var quitButton = new Button
        {
            Width=300,
            Height=75,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Quit",
                Font = _fontManager.GetOrbitronLightFont(20),
            }
        };
        Grid.SetRow(quitButton, 3);

        quitButton.Click += (s, a) =>
        {
            MyraEnvironment.Game.Exit();
        };

        grid.Widgets.Add(quitButton);
        
        _desktop = new Desktop();
        _desktop.Root = grid;
    }
    
    public override void Update(GameTime gameTime)
    {
        // Left intentionally blank.
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _desktop.Render();
    }
}