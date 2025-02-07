using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class AttributionsUi
{
    private readonly Desktop _desktop;

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
                                            "Settings image by Alexander Andrews on Unsplash");
        
        var panel = new VerticalStackPanel
        {
            Margin = new Thickness(180),
            Padding = new Thickness(10),
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
    
    private VerticalStackPanel ExitPanel(Game game)
    {
        var button = UiComponents.Button("Return to Main Menu");
        button.Click += (_, _) =>
        {
            game.SceneManager.ChangeScene(new MainMenuScene(game));
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

    public void Draw()
    {
        _desktop.Render();
    }
}