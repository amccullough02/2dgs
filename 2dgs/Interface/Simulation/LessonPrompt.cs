using System.Collections.Generic;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class LessonPrompt
{
    private Desktop _desktop;
    private SimulationData _simulationData;
    private readonly Window _window;
    private readonly List<LessonPage> _lessonPages;
    private readonly string _title;
    private int _index;
    private readonly int _numPages;
    
    public LessonPrompt(SimulationData simulationData)
    {
        _title = simulationData.SimulationTitle;
        _lessonPages = simulationData.LessonPages;
        _numPages = simulationData.LessonPages.Count;
        _window = LessonWindow();
    }
    
    public List<LessonPage> GetLessons => _lessonPages;

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
            Opacity = UiConstants.DefaultDialogOpacity,
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
        
        var textbox = UiComponents.ReadOnlyTextBox(_lessonPages[_index].Text);
        textbox.MinHeight = 250;
        
        return textbox;
    }

    private Grid PaginationControls(TextBox textBox)
    {
        var grid = UiComponents.Grid(0, 4, 1);
        
        var previousButton = UiComponents.Button("Previous Page", visible: false, width: 150, height: 50);
        Grid.SetColumn(previousButton, 0);
        
        var pageLabel = UiComponents.LightLabel($"Page {_index + 1} of {_numPages}");
        pageLabel.VerticalAlignment = VerticalAlignment.Center;
        pageLabel.Margin = new Thickness(50, 0, 50, 0);
        Grid.SetColumn(pageLabel, 1);
        
        var nextButton = UiComponents.Button("Next Page", width: 150, height: 50);
        Grid.SetColumn(nextButton, 2);

        var confirmReset = UiComponents.StyledDialog("Confirm Action");
        confirmReset.Content = UiComponents.LightLabel("Are you sure you want to reset the simulation?");
        
        confirmReset.ButtonOk.Click += (_, __) =>
        {
            _simulationData.ResetSimulation = true;
        };
        
        var resetButton = UiComponents.Button("Reset Simulation", visible: false, width: 150, height: 50);
        Grid.SetColumn(resetButton, 3);
        
        nextButton.Click += (s, e) =>
        {
            if (_index < _numPages - 1)
            {
                FindWidget.UnhighlightWidget(_desktop.Root, _lessonPages[_index].HighlightWidget);
                FindWidget.EnableWidgets(_desktop.Root, _lessonPages[_index].RestrictWidgets);
                _index++;
                textBox.Text = _lessonPages[_index].Text;
                pageLabel.Text = $"Page {_index + 1} of {_numPages}";
                FindWidget.HighlightWidget(_desktop.Root, _lessonPages[_index].HighlightWidget);
                FindWidget.DisableWidgets(_desktop.Root, _lessonPages[_index].RestrictWidgets);
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
                FindWidget.UnhighlightWidget(_desktop.Root, _lessonPages[_index].HighlightWidget);
                FindWidget.EnableWidgets(_desktop.Root, _lessonPages[_index].RestrictWidgets);
                _index--;
                FindWidget.HighlightWidget(_desktop.Root, _lessonPages[_index].HighlightWidget);
                FindWidget.DisableWidgets(_desktop.Root, _lessonPages[_index].RestrictWidgets);
                nextButton.Visible = true;
                resetButton.Visible = false;
            }
            if (_index == 0) previousButton.Visible = false;
            textBox.Text = _lessonPages[_index].Text;
            pageLabel.Text = $"Page {_index + 1} of {_numPages}";
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