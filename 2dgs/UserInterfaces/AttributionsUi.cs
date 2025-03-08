using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A class used to contain UI boilerplate for the Attributions Scene.
/// </summary>
public class AttributionsUi
{
    /// <summary>
    /// An instance of a Myra Desktop, the highest unit of organisation in Myra's UI system.
    /// </summary>
    private readonly Desktop _desktop;

    /// <summary>
    /// The constructor for the AttributionsUi class.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    public AttributionsUi(Game game)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        
        var title = UiComponents.TitleLabel("Attributions and Acknowledgements");
        
        rootContainer.Widgets.Add(title);
        rootContainer.Widgets.Add(CreateAttributions());
        rootContainer.Widgets.Add(ExitPanel(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    /// <summary>
    /// A helper method to create the attributions panel.
    /// </summary>
    /// <returns>A centred vertical stack panel containing attributions.</returns>
    private VerticalStackPanel CreateAttributions()
    {
        var musicSection = UiComponents.MediumLabel("Music:", 24);
        var music = UiComponents.LightLabel("Origami Hairball by Krzysztof Pikes\n" +
                                            "Long Way Down by Krzysztof Pikes\n" +
                                            "Suspended in Air by Krzysztof Pikes");
        music.Margin = new Thickness(0, 0, 0, 10);
        
        var imageSection = UiComponents.MediumLabel("Images:", 24);
        var image = UiComponents.LightLabel("Main Menu image by Bryan Goff on Unsplash\n" +
                                            "Attribution image by NASA on Unsplash\n" +
                                            "Settings image by Alexander Andrews on Unsplash\n" +
                                            "Simulation Menu image by Steve Busch on Unsplash\n" +
                                            "Simulation image by Thor Alvis on Unsplash");
        
        var panel = new VerticalStackPanel
        {
            Margin = new Thickness(180),
            Padding = new Thickness(UiConstants.DefaultLargePadding),
            Border = new SolidBrush(Color.White),
            BorderThickness = new Thickness(1),
            Background = new SolidBrush(Color.Black * 0.5f),
        };

        panel.Widgets.Add(musicSection);
        panel.Widgets.Add(music);
        panel.Widgets.Add(imageSection);
        panel.Widgets.Add(image);

        return panel;
    }
    
    /// <summary>
    /// A method to create a panel that contains the 'exit' button that returns to the main menu.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <returns>A vertical stack panel containing the exit button.</returns>
    private VerticalStackPanel ExitPanel(Game game)
    {
        var button = UiComponents.Button("Return to Main Menu");
        button.Click += (_, _) =>
        {
            game.SceneManager.PushScene(new FadeInScene(game, new MainMenuScene(game)));
        };

        var verticalStackPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(UiConstants.DefaultMargin),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
        
        verticalStackPanel.Widgets.Add(button);
        
        return verticalStackPanel;
    }

    /// <summary>
    /// Draws the Myra desktop.
    /// </summary>
    public void Draw()
    {
        _desktop.Render();
    }
}