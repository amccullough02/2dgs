using System;
using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SettingsMenuUi
{
    private Desktop _desktop;

    public SettingsMenuUi(Game game)
    {
        MyraEnvironment.Game = game;

        var rootContainer = new Panel();
        rootContainer.Widgets.Add(CreateSettingsMenu(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private Grid CreateSettingsMenu(Game game)
    {
        var grid = UiComponents.Grid(10, 1, 4);

        var settingsMenuTitle = UiComponents.Label("Settings Menu");

        var vsyncToggleButton =
            UiComponents.ToggleButton("Disable V-Sync", game._graphics.SynchronizeWithVerticalRetrace);
        vsyncToggleButton.Click += (s, e) =>
        {
            game._graphics.SynchronizeWithVerticalRetrace = vsyncToggleButton.IsToggled;
            game._graphics.ApplyChanges();
            Console.WriteLine("DEBUG: V-sync toggled: " + vsyncToggleButton.IsToggled);
            ((Label)vsyncToggleButton.Content).Text = vsyncToggleButton.IsToggled ? "Disable V-Sync" : "Enable V-Sync";
            Console.WriteLine("Actual status of V-sync: " + game._graphics.SynchronizeWithVerticalRetrace);
        };
        Grid.SetRow(vsyncToggleButton, 1);
        
        var showFpsToggleButton = UiComponents.ToggleButton("Hide FPS", true);
        showFpsToggleButton.Click += (s, e) =>
        {
            game._fpsCounter.ToggleFps();
            ((Label)showFpsToggleButton.Content).Text = showFpsToggleButton.IsToggled ? "Hide FPS" : "Show FPS";
        };
        Grid.SetRow(showFpsToggleButton, 2);
        
        var returnToMainMenuButton = UiComponents.Button("Return to Main Menu");
        returnToMainMenuButton.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new MainMenu(game));
        };
        Grid.SetRow(returnToMainMenuButton, 3);

        grid.Widgets.Add(settingsMenuTitle);
        grid.Widgets.Add(vsyncToggleButton);
        grid.Widgets.Add(showFpsToggleButton);
        grid.Widgets.Add(returnToMainMenuButton);

        return grid;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}