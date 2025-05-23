﻿using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A simple UI class containing the save, quit, and show lesson options for the simulation scene.
/// </summary>
public static class SaveQuitPanel
{
    /// <summary>
    /// Creates the set of save, quit, and lesson options.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    /// <param name="desktop">A reference to the desktop used in the SimulationUi class, for which this class is instantiated within.</param>
    /// <returns>A panel containing save, quit, and lesson options.</returns>
    public static VerticalStackPanel Create(Game game, SimulationMediator simulationMediator, Desktop desktop)
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
        
        var saveDialog = NameSimulationDialog(simulationMediator);
        
        var saveButton = UiComponents.Button("Save Simulation");
        
        saveButton.Click += (_, _) =>
        {
            if (!string.IsNullOrEmpty(simulationMediator.FilePath)) simulationMediator.AttemptToSaveFile = true;
            else saveDialog.Show(desktop);
        };
        
        saveAndQuitPanel.Widgets.Add(returnButton);
        if (!simulationMediator.Lesson) saveAndQuitPanel.Widgets.Add(saveButton);

        if (!simulationMediator.Lesson) return saveAndQuitPanel;
        {
            var promptButton = UiComponents.Button("Show Lesson Prompt");
            
            var prompt = new LessonPrompt(simulationMediator, promptButton);
            
            promptButton.Click += (_, _) =>
            {
                prompt.Show(desktop, simulationMediator);
                promptButton.Enabled = false;
            };
            
            saveAndQuitPanel.Widgets.Add(promptButton);
        }

        return saveAndQuitPanel;
    }
    
    /// <summary>
    /// A method used to create a dialog that allows the user to name their new simulation.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    /// <returns>A dialog allowing new simulation naming.</returns>
    private static Dialog NameSimulationDialog(SimulationMediator simulationMediator)
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
            simulationMediator.FilePath = newFilePath;
            simulationMediator.AttemptToSaveFile = true;
        };
        
        return newSimulationDialog;
    }
}