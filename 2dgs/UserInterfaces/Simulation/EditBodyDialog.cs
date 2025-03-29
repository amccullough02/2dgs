using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A simple UI class containing widgets for the edit body dialog.
/// </summary>
public static class EditBodyDialog
{
    /// <summary>
    /// A method to create the edit body dialog.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    /// <param name="desktop">A reference to the desktop used in the SimulationUi class, for which this class is instantiated within.</param>
    /// <returns>The edit body dialog.</returns>
    public static Dialog Create(SimulationMediator simulationMediator, Desktop desktop)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 7);
        
        var bodyNameLabel = UiComponents.MediumLabel("Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.TextBox("Default");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        bodyNameTextbox.Id = "bodyNameTextbox";

        var bodyPosXLabel = UiComponents.MediumLabel("Pos X: ");
        grid.Widgets.Add(bodyPosXLabel);
        Grid.SetRow(bodyPosXLabel, 1);
        var bodyPosXTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyPosXTextbox);
        Grid.SetColumn(bodyPosXTextbox, 1);
        Grid.SetRow(bodyPosXTextbox, 1);
        bodyPosXTextbox.Id = "bodyPosXTextbox";

        var bodyPosYLabel = UiComponents.MediumLabel("Pos Y: ");
        grid.Widgets.Add(bodyPosYLabel);
        Grid.SetRow(bodyPosYLabel, 2);
        var bodyPosYTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyPosYTextbox);
        Grid.SetColumn(bodyPosYTextbox, 1);
        Grid.SetRow(bodyPosYTextbox, 2);
        bodyPosYTextbox.Id = "bodyPosYTextbox";

        var bodyVelXLabel = UiComponents.MediumLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 3);
        var bodyVelXTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 3);
        bodyVelXTextbox.Id = "bodyVelXTextbox";

        var bodyVelYLabel = UiComponents.MediumLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 4);
        var bodyVelYTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 4);
        bodyVelYTextbox.Id = "bodyVelYTextbox";

        var bodyMassLabel = UiComponents.MediumLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 5);
        var bodyMassTextbox = UiComponents.TextBox("1e6");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 5);
        bodyMassTextbox.Id = "bodyMassTextbox";

        var bodyDiameterLabel = UiComponents.MediumLabel("Diameter: ");
        grid.Widgets.Add(bodyDiameterLabel);
        Grid.SetRow(bodyDiameterLabel, 6);
        var bodyDiameterTextbox = UiComponents.TextBox("0.05");
        grid.Widgets.Add(bodyDiameterTextbox);
        Grid.SetColumn(bodyDiameterTextbox, 1);
        Grid.SetRow(bodyDiameterTextbox, 6);
        bodyDiameterTextbox.Id = "bodyDiameterTextbox";

        var editBodyDialog = UiComponents.StyledDialog("Edit Body");
        editBodyDialog.Content = grid;
        
        var validationErrorDialog = UiComponents.StyledDialog("Validation Error");
        var validationErrorPanel = new VerticalStackPanel();
        var validationErrorHeader = UiComponents.MediumLabel("Your input was invalid, fix the following:\n");
        var validationErrorMessage = UiComponents.LightLabel("");
        validationErrorPanel.Widgets.Add(validationErrorHeader);
        validationErrorPanel.Widgets.Add(validationErrorMessage);
        validationErrorDialog.Content = validationErrorPanel;

        validationErrorDialog.ButtonOk.Click += (_, _) => { editBodyDialog.Show(desktop); };

        editBodyDialog.ButtonOk.Click += (_, _) =>
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(bodyNameTextbox.Text))
            {
                errors.Add("Body name must not be null or empty.");
            }
            
            if (!float.TryParse(bodyPosXTextbox.Text, out _))
            {
                errors.Add("Body position X must be a floating point number.");
            }
            
            if (!float.TryParse(bodyPosYTextbox.Text, out _))
            {
                errors.Add("Body position Y must be a floating point number.");
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
    
            if (!float.TryParse(bodyDiameterTextbox.Text, out _))
            {
                errors.Add("Body diameter must be an integer.");
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

            simulationMediator.EditBodyData.Name = bodyNameTextbox.Text;
            simulationMediator.EditBodyData.Position = new Vector2 { X = float.Parse(bodyPosXTextbox.Text), Y = float.Parse(bodyPosYTextbox.Text) };
            simulationMediator.EditBodyData.Velocity = new Vector2 { X = float.Parse(bodyVelXTextbox.Text), Y = float.Parse(bodyVelYTextbox.Text) };
            simulationMediator.EditBodyData.Mass = float.Parse(bodyMassTextbox.Text);
            simulationMediator.EditBodyData.Diameter = Convert.ToInt32(bodyDiameterTextbox.Text);
            simulationMediator.EditSelectedBody = true;
            
            editBodyDialog.Close();
        };

        return editBodyDialog;
    }
}