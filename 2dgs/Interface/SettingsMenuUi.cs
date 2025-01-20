using System;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Point = Microsoft.Xna.Framework.Point;

namespace _2dgs;

public class SettingsMenuUi
{
    private readonly Desktop _desktop;

    public SettingsMenuUi(Game game)
    {
        MyraEnvironment.Game = game;

        var rootContainer = new Panel();
        
        rootContainer.Widgets.Add(Settings(game));
        rootContainer.Widgets.Add(ExitPanel(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel Settings(Game game)
    {
        var settingsTitle = UiComponents.TitleLabel("Settings Menu");
        
        var settingsPanel = UiComponents.VerticalStackPanel(8,
            HorizontalAlignment.Center,
            VerticalAlignment.Top,
            new Thickness(0));
        
        settingsPanel.Widgets.Add(settingsTitle);
        settingsPanel.Widgets.Add(DisplaySettings(game));
        settingsPanel.Widgets.Add(MiscSettings(game));
        
        return settingsPanel;
    }

    private VerticalStackPanel DisplaySettings(Game game)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 3);
        
        var vsyncToggleLabel = UiComponents.Label("Toggle VSync");
        Grid.SetRow(vsyncToggleLabel, 0);
        
        var vsyncToggleButton =
            UiComponents.ToggleButton("V-Sync Enabled", game.Graphics.SynchronizeWithVerticalRetrace);
        Grid.SetColumn(vsyncToggleButton, 1);
        vsyncToggleButton.HorizontalAlignment = HorizontalAlignment.Right;
        vsyncToggleButton.Click += (s, e) =>
        {
            game.Graphics.SynchronizeWithVerticalRetrace = vsyncToggleButton.IsToggled;
            game.Graphics.ApplyChanges();
            Console.WriteLine("DEBUG: V-sync toggled: " + vsyncToggleButton.IsToggled);
            ((Label)vsyncToggleButton.Content).Text = vsyncToggleButton.IsToggled ? "V-Sync Enabled" : "V-Sync Disabled";
            Console.WriteLine("Actual status of V-sync: " + game.Graphics.SynchronizeWithVerticalRetrace);
        };
        
        var resolutionOptions = UiComponents.ComboView();
        Grid.SetRow(resolutionOptions, 1);
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("1920 x 1080"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("2560 x 1080"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("2560 x 1440"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("3440 x 1440"));
        resolutionOptions.Widgets.Add(UiComponents.DropdownLabel("3840 x 2160"));
        resolutionOptions.SelectedIndex = 0;
        resolutionOptions.Width = 150;

        var resolutionOptionButton = UiComponents.Button("Select", width: 150, height: 40);
        Grid.SetRow(resolutionOptionButton, 1);
        Grid.SetColumn(resolutionOptionButton, 1);
        
        resolutionOptionButton.Click += (s, e) =>
        {
            switch (resolutionOptions.SelectedIndex)
            {
                case 0:
                    Console.WriteLine("DEBUG: Switching to 1920 x 1080 resolution");
                    game.Graphics.PreferredBackBufferWidth = 1920;
                    game.Graphics.PreferredBackBufferHeight = 1080;
                    game.Graphics.ApplyChanges();
                    break;
                case 1:
                    Console.WriteLine("DEBUG: Switching to 2560 x 1080 resolution");
                    game.Graphics.PreferredBackBufferWidth = 2560;
                    game.Graphics.PreferredBackBufferHeight = 1080;
                    game.Graphics.ApplyChanges();
                    break;
                case 2:
                    Console.WriteLine("DEBUG: Switching to 2560 x 1440 resolution");
                    game.Graphics.PreferredBackBufferWidth = 2560;
                    game.Graphics.PreferredBackBufferHeight = 1440;
                    game.Graphics.ApplyChanges();
                    break;
                case 3:
                    Console.WriteLine("DEBUG: Switching to 3440 x 1440 resolution");
                    game.Graphics.PreferredBackBufferWidth = 3440;
                    game.Graphics.PreferredBackBufferHeight = 1440;
                    game.Graphics.ApplyChanges();
                    break;
                case 4:
                    Console.WriteLine("DEBUG: Switching to 3840 x 2160 resolution");
                    game.Graphics.PreferredBackBufferWidth = 3840;
                    game.Graphics.PreferredBackBufferHeight = 2160;
                    game.Graphics.ApplyChanges();
                    break;
            }
        };
        
        var windowOptions = UiComponents.ComboView();
        windowOptions.Widgets.Add(UiComponents.DropdownLabel("Windowed"));
        windowOptions.Widgets.Add(UiComponents.DropdownLabel("Fullscreen"));
        windowOptions.SelectedIndex = 0;
        windowOptions.Width = 150;
        Grid.SetRow(windowOptions, 2);
        
        var windowOptionButton = UiComponents.Button("Select", width: 150, height: 40);
        Grid.SetRow(windowOptionButton, 2);
        Grid.SetColumn(windowOptionButton, 1);
        windowOptionButton.Click += (s, e) =>
        {
            switch (windowOptions.SelectedIndex)
            {
                case 0:
                    game.Graphics.IsFullScreen = false;
                    game.Graphics.ApplyChanges();
                    game.Window.Position = new Point(100, 100);
                    Console.WriteLine("DEBUG: Switching to windowed mode");
                    break;
                case 1:
                    game.Graphics.IsFullScreen = true;
                    game.Graphics.ApplyChanges();
                    Console.WriteLine("DEBUG: Switching to fullscreen mode");
                    break;
            }
        };
        
        grid.Widgets.Add(vsyncToggleLabel);
        grid.Widgets.Add(vsyncToggleButton);
        grid.Widgets.Add(resolutionOptions);
        grid.Widgets.Add(resolutionOptionButton);
        grid.Widgets.Add(windowOptions);
        grid.Widgets.Add(windowOptionButton);
        
        var sectionTitle = UiComponents.Label("Display Settings");
        sectionTitle.HorizontalAlignment = HorizontalAlignment.Center;
        var divider = UiComponents.HorizontalSeparator();
        
        var panel = new VerticalStackPanel();
        
        panel.Widgets.Add(sectionTitle);
        panel.Widgets.Add(divider);
        panel.Widgets.Add(grid);

        return panel;
    }

    private VerticalStackPanel MiscSettings(Game game)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 1, 2);
        
        var showFpsLabel = UiComponents.Label("Toggle FPS Counter");
        
        var showFpsToggleButton = UiComponents.ToggleButton("FPS Enabled", true);
        showFpsToggleButton.HorizontalAlignment = HorizontalAlignment.Right;
        showFpsToggleButton.Click += (s, e) =>
        {
            game.FpsCounter.ToggleFps();
            ((Label)showFpsToggleButton.Content).Text = showFpsToggleButton.IsToggled ? "FPS Enabled" : "FPS Disabled";
        };
        Grid.SetColumn(showFpsToggleButton, 1);
        
        grid.Widgets.Add(showFpsLabel);
        grid.Widgets.Add(showFpsToggleButton);

        var sectionTitle = UiComponents.Label("Miscellaneous Settings");
        sectionTitle.HorizontalAlignment = HorizontalAlignment.Center;
        var divider = UiComponents.HorizontalSeparator();
        
        var panel = new VerticalStackPanel();
        panel.Widgets.Add(sectionTitle);
        panel.Widgets.Add(divider);
        panel.Widgets.Add(grid);
        
        return panel;
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

    public void Draw()
    {
        _desktop.Render();
    }
}