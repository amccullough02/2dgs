using System.Numerics;
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
        
        var validationErrorMessage = UiComponents.MediumLabel("Validation Error: ");
        var validationErrorDialogue = UiComponents.ValidationWindow(validationErrorMessage);
        validationErrorDialogue.CloseButton.Click += (s, e) => { createBodyDialog.Show(desktop); };
    
        createBodyDialog.ButtonOk.Click += (sender, e) =>
        {
            var valid = true;
            var errorMessage = "";
    
            if (bodyNameTextbox.Text == null)
            {
                valid = false;
                errorMessage = "Name must be at least 1 character.";
            }
    
            else if (!float.TryParse(bodyVelXTextbox.Text, out _))
            {
                valid = false;
                errorMessage = "Vel X must be a number.";
            }
    
            else if (!float.TryParse(bodyVelYTextbox.Text, out _))
            {
                valid = false;
                errorMessage = "Vel Y must be a number.";
            }
    
            else if (!float.TryParse(bodyMassTextbox.Text, out _))
            {
                valid = false;
                errorMessage = "Mass must be a number.";
            }
    
            else if (!float.TryParse(bodyDisplaySizeTextbox.Text, out _))
            {
                valid = false;
                errorMessage = "Display Size must be a number.";
            }
    
            if (valid)
            {
                var name = bodyNameTextbox.Text;
                var velocity = new Vector2(float.Parse(bodyVelXTextbox.Text), float.Parse(bodyVelYTextbox.Text));
                var mass = float.Parse(bodyMassTextbox.Text);
                var size = float.Parse(bodyDisplaySizeTextbox.Text);
    
                simulationMediator.CreateBodyData.Name = name;
                simulationMediator.CreateBodyData.Velocity = velocity;
                simulationMediator.CreateBodyData.Mass = mass;
                simulationMediator.CreateBodyData.DisplaySize = size;
                simulationMediator.ToggleBodyGhost = true;
                
                createBodyDialog.Close();
            }
            else
            {
                validationErrorMessage.Text = errorMessage;
                validationErrorDialogue.Show(desktop);
            }
        };
    
        return createBodyDialog;
    }
}