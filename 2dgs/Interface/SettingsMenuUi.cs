using System;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SettingsMenuUi
{
    private Desktop _desktop;

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
        settingsTitle.Padding = new Thickness(UiConstants.DefaultMargin);
        
        var settingsPanel = UiComponents.VerticalStackPanel(8,
            HorizontalAlignment.Center,
            VerticalAlignment.Top,
            new Thickness(UiConstants.DefaultMargin));
        
        settingsPanel.Widgets.Add(settingsTitle);
        settingsPanel.Widgets.Add(DisplaySettings(game));
        settingsPanel.Widgets.Add(MiscSettings(game));
        
        return settingsPanel;
    }

    private VerticalStackPanel DisplaySettings(Game game)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 3, 2);
        
        var vsyncToggleLabel = UiComponents.Label("Toggle VSync");
        Grid.SetRow(vsyncToggleLabel, 0);
        
        var vsyncToggleButton =
            UiComponents.ToggleButton("V-Sync Enabled", game._graphics.SynchronizeWithVerticalRetrace);
        vsyncToggleButton.HorizontalAlignment = HorizontalAlignment.Right;
        vsyncToggleButton.Click += (s, e) =>
        {
            game._graphics.SynchronizeWithVerticalRetrace = vsyncToggleButton.IsToggled;
            game._graphics.ApplyChanges();
            Console.WriteLine("DEBUG: V-sync toggled: " + vsyncToggleButton.IsToggled);
            ((Label)vsyncToggleButton.Content).Text = vsyncToggleButton.IsToggled ? "V-Sync Enabled" : "V-Sync Disabled";
            Console.WriteLine("Actual status of V-sync: " + game._graphics.SynchronizeWithVerticalRetrace);
        };
        Grid.SetRow(vsyncToggleButton, 0);
        Grid.SetColumn(vsyncToggleButton, 1);
        
        grid.Widgets.Add(vsyncToggleLabel);
        grid.Widgets.Add(vsyncToggleButton);
        
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
            game._fpsCounter.ToggleFps();
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