using System.Numerics;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class EditBodyDialog
{
    public static Dialog Create(SimulationData simulationData, Desktop desktop)
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

        var bodyDisplaySizeLabel = UiComponents.MediumLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 6);
        var bodyDisplaySizeTextbox = UiComponents.TextBox("0.05");
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 6);
        bodyDisplaySizeTextbox.Id = "bodyDisplaySizeTextbox";

        var editBodyDialog = UiComponents.StyledDialog("Edit Body");
        editBodyDialog.Content = grid;
        var validationErrorMessage = UiComponents.MediumLabel("Validation Error: ");
        var validationErrorDialogue = UiComponents.ValidationWindow(validationErrorMessage);
        validationErrorDialogue.CloseButton.Click += (s, e) => { editBodyDialog.Show(desktop); };

        editBodyDialog.ButtonOk.Click += (sender, e) =>
        {
            var valid = true;
            var errorMessage = "";

            if (bodyNameTextbox.Text.Length < 2)
            {
                valid = false;
                errorMessage = "Name must be at least 2 characters.";
            }
            
            else if (!float.TryParse(bodyPosXTextbox.Text, out _))
            {
                valid = false;
                errorMessage = "Pos X must be a number.";
            }
            
            else if (!float.TryParse(bodyPosYTextbox.Text, out _))
            {
                valid = false;
                errorMessage = "Pos Y must be a number.";
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
                var position = new Vector2(float.Parse(bodyPosXTextbox.Text), float.Parse(bodyPosYTextbox.Text));
                var velocity = new Vector2(float.Parse(bodyVelXTextbox.Text), float.Parse(bodyVelYTextbox.Text));
                var mass = float.Parse(bodyMassTextbox.Text);
                var size = float.Parse(bodyDisplaySizeTextbox.Text);

                simulationData.EditBodyData.Name = name;
                simulationData.EditBodyData.Position = position;
                simulationData.EditBodyData.Velocity = velocity;
                simulationData.EditBodyData.Mass = mass;
                simulationData.EditBodyData.DisplaySize = size;
                simulationData.EditSelectedBody = true;
                
                editBodyDialog.Close();
            }
            else
            {
                validationErrorMessage.Text = errorMessage;
                validationErrorDialogue.Show(desktop);
            }
        };

        return editBodyDialog;
    }
}