using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A simple UI class containing widgets for the create body dialog.
/// </summary>
public static class CreateBodyDialog
{
    /// <summary>
    /// A method to create the create body dialog.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    /// <param name="desktop">A reference to the desktop used in the SimulationUi class, for which this class is instantiated within.</param>
    /// <returns>The create body dialog.</returns>
    public static Dialog Create(SimulationMediator simulationMediator, Desktop desktop)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 5);
        
        var bodyNameLabel = UiComponents.MediumLabel("Body Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.HintTextBox("e.g. Arrakis");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        
        var bodyVelXLabel = UiComponents.MediumLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 1);
        var bodyVelXTextbox = UiComponents.HintTextBox("e.g. 3.1, -2.52");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 1);
        
        var bodyVelYLabel = UiComponents.MediumLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 2);
        var bodyVelYTextbox = UiComponents.HintTextBox("e.g. 3.1, -2.52");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 2);
        
        var bodyMassLabel = UiComponents.MediumLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 3);
        var bodyMassTextbox = UiComponents.HintTextBox("e.g. 1e6, 1000000");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 3);
    
        var bodyDisplaySizeLabel = UiComponents.MediumLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 4);
        var bodyDisplaySizeTextbox = UiComponents.HintTextBox("e.g. 0.02, 0.08");
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 4);
        
        var createBodyDialog = UiComponents.StyledDialog("Create New Body");
        createBodyDialog.Content = grid;
        
        var validationErrorDialog = UiComponents.StyledDialog("Validation Error");
        var validationErrorPanel = new VerticalStackPanel();
        var validationErrorHeader = UiComponents.MediumLabel("Your input was invalid, fix the following:\n");
        var validationErrorMessage = UiComponents.LightLabel("");
        validationErrorPanel.Widgets.Add(validationErrorHeader);
        validationErrorPanel.Widgets.Add(validationErrorMessage);
        validationErrorDialog.Content = validationErrorPanel;

        validationErrorDialog.ButtonOk.Click += (_, _) => { createBodyDialog.Show(desktop); };
    
        createBodyDialog.ButtonOk.Click += (_, _) =>
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(bodyNameTextbox.Text))
            {
                errors.Add("Body name must not be null or empty.");
            }

            if (!float.TryParse(bodyVelXTextbox.Text, out _))
            {
                errors.Add("Body velocity X must be a floating point number.");
            }
        
            if (!float.TryParse(bodyVelYTextbox.Text, out _))
            {
                errors.Add("Body velocity Y must be a floating point number.");
            }
            
            if (!float.TryParse(bodyMassTextbox.Text, out _))
            {
                errors.Add("Body mass must be a floating point number.");
            }
    
            if (!float.TryParse(bodyDisplaySizeTextbox.Text, out _))
            {
                errors.Add("Body display size must be a floating point number.");
            }

            if (errors.Count != 0)
            {
                var errorMessage = "";
                
                for (var i = 0; i < errors.Count; i++)
                {
                    errorMessage += (i + 1) + ". " + errors[i] + "\n";
                }
                
                validationErrorMessage.Text = errorMessage;
                validationErrorDialog.Show(desktop);

                return;
            }

            simulationMediator.CreateBodyData.Name = bodyNameTextbox.Text;
            simulationMediator.CreateBodyData.Velocity = new Vector2 { X = float.Parse(bodyVelXTextbox.Text), Y = float.Parse(bodyVelYTextbox.Text) };
            simulationMediator.CreateBodyData.Mass = float.Parse(bodyMassTextbox.Text);
            simulationMediator.CreateBodyData.Diameter = Convert.ToInt32(bodyDisplaySizeTextbox.Text);
            simulationMediator.ToggleBodyGhost = true;
            
            createBodyDialog.Close();
        };
    
        return createBodyDialog;
    }
}