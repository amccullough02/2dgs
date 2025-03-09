using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A class used to represent a 2DGS 'Lesson Prompt' window, which is used to discuss various physics concepts.
/// </summary>
public class LessonPrompt
{
    /// <summary>
    /// An instance of the Myra Desktop which is deliberately not initialised, so it can be used as a reference later on.
    /// </summary>
    private Desktop _desktop;
    /// <summary>
    /// An instance of the SimulationMediator class.
    /// </summary>
    private SimulationMediator _simulationMediator;
    /// <summary>
    /// The Window from which the lesson prompt will be based on.
    /// </summary>
    private readonly Window _window;
    /// <summary>
    /// The lesson content.
    /// </summary>
    private readonly List<LessonPage> _lessonPages;
    /// <summary>
    /// The title of the lesson.
    /// </summary>
    private readonly string _title;
    /// <summary>
    /// The current page index.
    /// </summary>
    private int _index;
    /// <summary>
    /// The number of pages.
    /// </summary>
    private readonly int _numPages;
    
    /// <summary>
    /// The LessonPrompt constructor.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator.</param>
    /// <param name="showButton">A reference to the button that shows the Lesson Prompt, required to disable that button if the prompt is active.</param>
    public LessonPrompt(SimulationMediator simulationMediator, Button showButton)
    {
        _title = simulationMediator.SimulationTitle;
        _lessonPages = simulationMediator.LessonPages;
        _numPages = simulationMediator.LessonPages.Count;
        _window = LessonWindow(showButton);
    }

    /// <summary>
    /// A method to construct the lesson window proper.
    /// </summary>
    /// <param name="showButton">A reference to the button that shows the Lesson Prompt, required to disable that button if the prompt is active.</param>
    /// <returns></returns>
    private Window LessonWindow(Button showButton)
    {
        var window = new Window
        {
            Title = _title,
            TitleFont = FontManager.MediumText(UiConstants.DefaultFontSize),
            TitlePanel =
            {
                HorizontalAlignment = HorizontalAlignment.Center, Padding = new Thickness(0,
                    UiConstants.DefaultMargin,
                    0,
                    0)
            },
            Width = 600,
            Height = 400,
            Background = new SolidBrush(Color.Black * UiConstants.DefaultLessonOpacity),
            Border = new SolidBrush(Color.White),
            BorderThickness = new Thickness(1),
            Content = LessonLayout()
        };

        window.CloseButton.Click += (_, _) =>
        {
            showButton.Enabled = true;
        };

        return window;
    }

    /// <summary>
    /// A method that constructs the contents of the lesson window.
    /// </summary>
    /// <returns>A vertical stack panel that contains the lesson prompt content.</returns>
    private VerticalStackPanel LessonLayout()
    {
        var verticalStackPanel = new VerticalStackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Padding = new Thickness(UiConstants.DefaultLargePadding)
        };

        var textBox = LessonContent();
        
        verticalStackPanel.Widgets.Add(textBox);
        verticalStackPanel.Widgets.Add(PaginationControls(textBox));
        
        return verticalStackPanel;
    }

    /// <summary>
    /// A method that creates a read-only textbox containing the written material on the lesson.
    /// </summary>
    /// <returns>A read-only textbox containing the written material on the lesson.</returns>
    private TextBox LessonContent()
    {
        
        var textbox = UiComponents.ReadOnlyTextBox(_lessonPages[_index].Text);
        textbox.MinHeight = 250;
        
        return textbox;
    }

    /// <summary>
    /// A method that creates the pagination controls for the lesson prompt.
    /// </summary>
    /// <param name="textBox">A reference to the read-only textbox so its contents can be altered.</param>
    /// <returns>A grid containing the pagination controls for the lesson prompt.</returns>
    private Grid PaginationControls(TextBox textBox)
    {
        var grid = UiComponents.Grid(0, 4, 1);
        
        var previousButton = UiComponents.Button("Previous Page", visible: false, width: 175, height: 50);
        Grid.SetColumn(previousButton, 0);
        
        var pageLabel = UiComponents.LightLabel($"Page {_index + 1} of {_numPages}");
        pageLabel.Margin = new Thickness(55, 0);
        pageLabel.HorizontalAlignment = HorizontalAlignment.Center;
        Grid.SetColumn(pageLabel, 1);
        
        var nextButton = UiComponents.Button("Next Page", width: 175, height: 50);
        Grid.SetColumn(nextButton, 2);

        var confirmReset = UiComponents.StyledDialog("Confirm Action");
        confirmReset.Content = UiComponents.LightLabel("Are you sure you want to reset the simulation?");
        
        confirmReset.ButtonOk.Click += (_, _) =>
        {
            _simulationMediator.ResetSimulation = true;
        };
        
        var resetButton = UiComponents.Button("Reset Simulation", visible: false, width: 175, height: 50);
        Grid.SetColumn(resetButton, 3);
        
        nextButton.Click += (_, _) =>
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
        
        previousButton.Click += (_, _) =>
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
        
        resetButton.Click += (_, _) =>
        {
            confirmReset.Show(_desktop);
        };
        
        grid.Widgets.Add(previousButton);
        grid.Widgets.Add(pageLabel);
        grid.Widgets.Add(nextButton);
        grid.Widgets.Add(resetButton);
        
        return grid;
    }

    /// <summary>
    /// A method to display the lesson prompt.
    /// </summary>
    /// <param name="desktop">A reference to the SimulationUi desktop, this is set as the value of the desktop instance within this class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    public void Show(Desktop desktop, SimulationMediator simulationMediator)
    {
        _simulationMediator = simulationMediator;
        _desktop = desktop;
        _window.Show(desktop);
    }
}