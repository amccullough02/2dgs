using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class SaveQuitPanel
{
    public static VerticalStackPanel Create(SimulationData simulationData, Game game, Desktop desktop)
    {
        var saveAndQuitPanel = UiComponents.VerticalStackPanel(8, HorizontalAlignment.Right, VerticalAlignment.Top,
            new Thickness(0, UiConstants.DefaultMargin, UiConstants.DefaultMargin, 0));
        
        var returnButton = UiComponents.Button("Exit Simulation");
        returnButton.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        var saveButton = UiComponents.Button("Save Simulation");
        saveButton.Click += (s, e) =>
        {
            simulationData.AttemptToSaveFile = true;
        };
        
        saveAndQuitPanel.Widgets.Add(returnButton);
        saveAndQuitPanel.Widgets.Add(saveButton);

        if (!simulationData.IsLesson) return saveAndQuitPanel;
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