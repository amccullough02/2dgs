using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SettingsMenu : GameState
{
    private Desktop _desktop;
    private FontManager _fontManager;
    private Game game;
    
    public SettingsMenu(Game game)
    {
        this.game = game;
        
        _fontManager = new FontManager();

        MyraEnvironment.Game = this.game;

        var grid = new Grid
        {
            RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        // COLUMNS
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        
        // ROWS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // TITLE
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // V-SYNC
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // SHOW FPS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // RETURN TO MAIN MENU

        var title = new Label
        {
            Id = "title",
            Text = "Settings Menu",
            Font = _fontManager.GetOrbitronLightFont(70),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, 50, 0, 0),
        };
        Grid.SetRow(title, 0);
        grid.Widgets.Add(title);

        var vsyncButton = new ToggleButton
        {
            Id = "vsync",
            IsToggled = this.game._graphics.SynchronizeWithVerticalRetrace,
            Content = new Label
            {
                Id = "vsyncLabel",
                Text = "Disable V-Sync"
            }
        };
        Grid.SetRow(vsyncButton, 1);
        grid.Widgets.Add(vsyncButton);

        vsyncButton.Click += (s, e) =>
        {
            this.game._graphics.SynchronizeWithVerticalRetrace = vsyncButton.IsToggled;
            this.game._graphics.ApplyChanges();
            Console.WriteLine("DEBUG: V-sync toggled: " + vsyncButton.IsToggled);
            ((Label)vsyncButton.Content).Text = vsyncButton.IsToggled ? "Disable V-Sync" : "Enable V-Sync";
            Console.WriteLine("Actual status of V-sync: " + this.game._graphics.SynchronizeWithVerticalRetrace);
        };

        var showFpsButton = new ToggleButton
        {
            Id = "showFPS",
            IsToggled = true,
            Content = new Label
            {
                Id = "showFPSLabel",
                Text = "Hide FPS"
            }
        };
        Grid.SetRow(showFpsButton, 2);
        grid.Widgets.Add(showFpsButton);

        showFpsButton.Click += (s, e) =>
        {
            this.game._fpsCounter.ToggleFps();
            ((Label)showFpsButton.Content).Text = showFpsButton.IsToggled ? "Hide FPS" : "Show FPS";
        };
        
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
        Grid.SetRow(returnButton, 3);

        returnButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to main menu...");
            this.game.GameStateManager.ChangeState(new MainMenu(this.game));
        };

        grid.Widgets.Add(returnButton);
        
        _desktop = new Desktop();
        _desktop.Root = grid;
    }

    public override void Initialize()
    {
        
    }

    public override void LoadContent(ContentManager content)
    {
        
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