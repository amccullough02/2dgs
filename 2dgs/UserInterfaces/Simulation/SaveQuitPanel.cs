﻿using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class SaveQuitPanel
{
    public static VerticalStackPanel Create(SimulationSceneData simulationSceneData, Game game, Desktop desktop)
    {
        var saveAndQuitPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(0, UiConstants.DefaultMargin, UiConstants.DefaultMargin, 0),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        };
        
        var returnButton = UiComponents.Button("Exit Simulation");
        returnButton.Click += (_, _) =>
        {
            game.SceneManager.PushScene(new FadeInScene(game, new SimulationMenuScene(game)));
        };
        
        var saveDialog = NameSimulationDialog(simulationSceneData);
        
        var saveButton = UiComponents.Button("Save Simulation");
        saveButton.Click += (_, _) =>
        {
            if (!string.IsNullOrEmpty(simulationSceneData.FilePath)) simulationSceneData.AttemptToSaveFile = true;
            else saveDialog.Show(desktop);
        };
        
        saveAndQuitPanel.Widgets.Add(returnButton);
        saveAndQuitPanel.Widgets.Add(saveButton);

        if (!simulationSceneData.Lesson) return saveAndQuitPanel;
        {
            var prompt = new LessonPrompt(simulationSceneData);
        
            UiTests.TestLessonPrompt(simulationSceneData.LessonPages, prompt.GetLessons);
        
            var promptButton = UiComponents.Button("Show Lesson Prompt");
            promptButton.Click += (_, _) =>
            {
                prompt.Show(desktop, simulationSceneData);
            };
            
            saveAndQuitPanel.Widgets.Add(promptButton);
        }

        return saveAndQuitPanel;
    }
    
    private static Dialog NameSimulationDialog(SimulationSceneData simulationSceneData)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 1);
        var nameSimulationLabel = UiComponents.MediumLabel("Simulation Name: ");
        Grid.SetColumn(nameSimulationLabel, 0);

        var nameSimulationTextbox = UiComponents.TextBox("new_simulation");
        Grid.SetColumn(nameSimulationTextbox, 1);
        
        grid.Widgets.Add(nameSimulationLabel);
        grid.Widgets.Add(nameSimulationTextbox);
        
        var newSimulationDialog = UiComponents.StyledDialog("Save Simulation As");
        newSimulationDialog.Content = grid;
        newSimulationDialog.ButtonOk.Click += (_, _) =>
        {
            var newFilePath = "../../../savedata/my_simulations/" + nameSimulationTextbox.Text + ".json";
            simulationSceneData.FilePath = newFilePath;
            simulationSceneData.AttemptToSaveFile = true;
        };
        
        return newSimulationDialog;
    }
}