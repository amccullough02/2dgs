using System;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class MainMenuUi
{
    private Desktop _desktop;

    public MainMenuUi(Game game)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        var menuTitle = UiComponents.TitleLabel("2D Gravity Simulator");
        
        rootContainer.Widgets.Add(CreateMainMenu(game));
        rootContainer.Widgets.Add(menuTitle);
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel CreateMainMenu(Game game)
    {
        var mainMenuStackPanel = UiComponents.VerticalStackPanel(8, HorizontalAlignment.Center,
            VerticalAlignment.Center, new Thickness(UiConstants.DefaultMargin));

        var simulationMenuButton = UiComponents.Button("Simulation Menu");
        simulationMenuButton.Click += (s, e) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation menu...");
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        var settingsMenuButton = UiComponents.Button("Settings");
        settingsMenuButton.Click += (s, a) =>
        {
            Console.WriteLine("DEBUG: Navigating to settings menu...");
            game.GameStateManager.ChangeState(new SettingsMenu(game));
        };
        
        var quitButton = UiComponents.Button("Quit");
        quitButton.Click += (s, a) =>
        {
            MyraEnvironment.Game.Exit();
        };
        
        mainMenuStackPanel.Widgets.Add(simulationMenuButton);
        mainMenuStackPanel.Widgets.Add(settingsMenuButton);
        mainMenuStackPanel.Widgets.Add(quitButton);
        
        return mainMenuStackPanel;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}