using System;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class MainMenuUi
{
    private readonly Desktop _desktop;

    public MainMenuUi(Game game)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        
        rootContainer.Widgets.Add(CreateMainMenu(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel CreateMainMenu(Game game)
    {
        var mainMenuStackPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(8),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };
        
        var newSimulationButton = UiComponents.MenuButton("New Simulation", width: UiConstants.DefaultMenuButtonWidth);
        newSimulationButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Creating a blank simulation...");
            game.SceneManager.ChangeScene(new SimulationScene(game, null));
        };

        var simulationMenuButton = UiComponents.MenuButton("Browse Simulations", width: UiConstants.DefaultMenuButtonWidth);
        simulationMenuButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation menu...");
            game.SceneManager.ChangeScene(new SimulationMenuScene(game));
        };
        
        var attributionsButton = UiComponents.MenuButton("Attributions", width: UiConstants.DefaultMenuButtonWidth);
        attributionsButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to attributions menu...");
            game.SceneManager.ChangeScene(new AttributionsScene(game));
        };
        
        var settingsMenuButton = UiComponents.MenuButton("Settings", width: UiConstants.DefaultMenuButtonWidth);
        settingsMenuButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to settings menu...");
            game.SceneManager.ChangeScene(new SettingsScene(game));
        };
        
        var quitButton = UiComponents.MenuButton("Quit", width: UiConstants.DefaultMenuButtonWidth);
        quitButton.Click += (_, _) =>
        {
            MyraEnvironment.Game.Exit();
        };
        
        mainMenuStackPanel.Widgets.Add(newSimulationButton);
        mainMenuStackPanel.Widgets.Add(simulationMenuButton);
        mainMenuStackPanel.Widgets.Add(attributionsButton);
        mainMenuStackPanel.Widgets.Add(settingsMenuButton);
        mainMenuStackPanel.Widgets.Add(quitButton);
        
        return mainMenuStackPanel;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}