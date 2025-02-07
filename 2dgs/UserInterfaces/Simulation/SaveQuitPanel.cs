using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class SaveQuitPanel
{
    public static VerticalStackPanel Create(SimulationData simulationData, Game game, Desktop desktop)
    {
        var saveAndQuitPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(0, UiConstants.DefaultMargin, UiConstants.DefaultMargin, 0),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        };
        
        var returnButton = UiComponents.Button("Exit Simulation");
        returnButton.Click += (s, e) =>
        {
            game.SceneManager.ChangeScene(new SimulationMenuScene(game));
        };
        
        var saveButton = UiComponents.Button("Save Simulation");
        saveButton.Click += (s, e) =>
        {
            simulationData.AttemptToSaveFile = true;
        };
        
        saveAndQuitPanel.Widgets.Add(returnButton);
        saveAndQuitPanel.Widgets.Add(saveButton);

        if (!simulationData.Lesson) return saveAndQuitPanel;
        {
            var prompt = new LessonPrompt(simulationData);
        
            UiTests.TestLessonPrompt(simulationData.LessonPages, prompt.GetLessons);
        
            var promptButton = UiComponents.Button("Show Lesson Prompt");
            promptButton.Click += (s, e) =>
            {
                prompt.Show(desktop, simulationData);
            };
            
            saveAndQuitPanel.Widgets.Add(promptButton);
        }

        return saveAndQuitPanel;
    }
}