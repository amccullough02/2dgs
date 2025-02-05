using System;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
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
        var mainMenuStackPanel = UiComponents.VerticalStackPanel(8, HorizontalAlignment.Center,
            VerticalAlignment.Center, new Thickness(UiConstants.DefaultMargin));

        var simulationMenuButton = UiComponents.Button("Simulation Menu");
        simulationMenuButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to simulation menu...");
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        var attributionsButton = UiComponents.Button("Attributions");
        attributionsButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to attributions menu...");
            game.GameStateManager.ChangeState(new Attributions(game));
        };
        
        var settingsMenuButton = UiComponents.Button("Settings");
        settingsMenuButton.Click += (_, _) =>
        {
            Console.WriteLine("DEBUG: Navigating to settings menu...");
            game.GameStateManager.ChangeState(new SettingsMenu(game));
        };
        
        var quitButton = UiComponents.Button("Quit");
        quitButton.Click += (_, _) =>
        {
            MyraEnvironment.Game.Exit();
        };
        
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