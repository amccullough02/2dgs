using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class LessonPrompt
{
    private Desktop _desktop;
    private SimulationData _simulationData;
    private Window _window;
    private string[] _lessonContent;
    private string _title;
    private int _index;
    private int _numPages;
    
    public LessonPrompt(SimulationData simulationData)
    {
        _lessonContent = simulationData.LessonContent;
        _title = simulationData.SimulationTitle;
        _numPages = _lessonContent.Length;
        _window = LessonWindow();
    }
    
    public string[] GetLessonContent => _lessonContent;

    private Window LessonWindow()
    {
        var window = new Window
        {
            Title = _title,
            TitleFont = FontManager.MediumFont(UiConstants.DefaultFontSize),
            TitlePanel =
            {
                HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0,
                    UiConstants.DefaultMargin,
                    0,
                    0)
            },
            Width = 600,
            Height = 400,
            Opacity = 0.9f,
            Content = LessonLayout()
        };

        return window;
    }

    private VerticalStackPanel LessonLayout()
    {
        var verticalStackPanel = new VerticalStackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Padding = new Thickness(10)
        };

        TextBox textBox = LessonContent();
        
        verticalStackPanel.Widgets.Add(textBox);
        verticalStackPanel.Widgets.Add(PaginationControls(textBox));
        
        return verticalStackPanel;
    }

    private TextBox LessonContent()
    {
        var textbox = new TextBox
        {
            Font = FontManager.LightFont(UiConstants.DefaultFontSize),
            Text = _lessonContent[_index],
            Multiline = true,
            Readonly = true,
            Wrap = true,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            BlinkIntervalInMs = 2000,
            MinHeight = 250
        };

        return textbox;
    }

    private Grid PaginationControls(TextBox textBox)
    {
        var grid = UiComponents.CreateGrid(0, 4, 1);
        
        var previousButton = UiComponents.CreateButton("Previous Button", visible: false, width: 150, height: 50);
        Grid.SetColumn(previousButton, 0);
        
        var pageLabel = UiComponents.CreateStyledLabel($"Page {_index + 1} of {_numPages}");
        pageLabel.VerticalAlignment = VerticalAlignment.Center;
        pageLabel.Margin = new Thickness(50, 0, 50, 0);
        Grid.SetColumn(pageLabel, 1);
        
        var nextButton = UiComponents.CreateButton("Next Button", width: 150, height: 50);
        Grid.SetColumn(nextButton, 2);
        
        var confirmReset = new Dialog { Title = "Confirm Action", 
            Content = new Label { Text = "Are you sure you want to reset the lesson?" }};
        confirmReset.ButtonOk.Click += (_, __) =>
        {
            _simulationData.ResetSimulation = true;
        };
        
        var resetButton = UiComponents.CreateButton("Reset Button", visible: false, width: 150, height: 50);
        Grid.SetColumn(resetButton, 3);
        
        nextButton.Click += (s, e) =>
        {
            if (_index < _numPages - 1)
            {
                _index++;
                textBox.Text = _lessonContent[_index];
                pageLabel.Text = $"Page {_index + 1} of {_numPages}";
                nextButton.Visible = true;
                previousButton.Visible = true;
            }

            if (_index == _numPages - 1)
            {
                nextButton.Visible = false;
                resetButton.Visible = true;
            }
        };
        
        previousButton.Click += (s, e) =>
        {
            if (_index > 0)
            {
                _index--;
                nextButton.Visible = true;
                resetButton.Visible = false;
            }
            if (_index == 0) previousButton.Visible = false;
            textBox.Text = _lessonContent[_index];
            pageLabel.Text = $"Page {_index + 1} of {_lessonContent.Length}";
        };
        
        resetButton.Click += (s, e) =>
        {
            confirmReset.Show(_desktop);
        };
        
        grid.Widgets.Add(previousButton);
        grid.Widgets.Add(pageLabel);
        grid.Widgets.Add(nextButton);
        grid.Widgets.Add(resetButton);
        
        return grid;
    }

    public void Show(Desktop desktop, SimulationData simulationData)
    {
        _simulationData = simulationData;
        _desktop = desktop;
        _window.Show(desktop);
    }
}