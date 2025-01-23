using System.Numerics;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class CreateBodyDialog
{
    public static Dialog Create(SimulationData simulationData, Desktop desktop)
    {
        var grid = UiComponents.Grid(UiConstants.DefaultGridSpacing, 2, 5);
        
        var bodyNameLabel = UiComponents.MediumLabel("Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.TextBox("Default name");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        
        var bodyVelXLabel = UiComponents.MediumLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 1);
        var bodyVelXTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 1);
        
        var bodyVelYLabel = UiComponents.MediumLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 2);
        var bodyVelYTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 2);
        
        var bodyMassLabel = UiComponents.MediumLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 3);
        var bodyMassTextbox = UiComponents.TextBox("1e6");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 3);
    
        var bodyDisplaySizeLabel = UiComponents.MediumLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 4);
        var bodyDisplaySizeTextbox = UiComponents.TextBox("0.05");
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
            bool valid = true;
            string errorMessage = "";
    
            if (bodyNameTextbox.Text.Length < 2)
            {
                valid = false;
                errorMessage = "Name must be at least 2 characters.";
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
                string name = bodyNameTextbox.Text;
                Vector2 velocity = new Vector2(float.Parse(bodyVelXTextbox.Text), float.Parse(bodyVelYTextbox.Text));
                float mass = float.Parse(bodyMassTextbox.Text);
                float size = float.Parse(bodyDisplaySizeTextbox.Text);
    
                simulationData.CreateBodyData.Name = name;
                simulationData.CreateBodyData.Velocity = velocity;
                simulationData.CreateBodyData.Mass = mass;
                simulationData.CreateBodyData.DisplayRadius = size;
                simulationData.ToggleBodyGhost = true;
                
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