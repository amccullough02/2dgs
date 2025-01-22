using System.Globalization;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace _2dgs;

public static class EditPanel
{
    public static VerticalStackPanel Create(SimulationData simulationData, Desktop desktop)
    {
        var deleteBodyButton = UiComponents.Button("Delete Body", false);
        deleteBodyButton.Click += (sender, args) =>
        {
            if (simulationData.EditMode && simulationData.IsABodySelected)
            {
                simulationData.DeleteSelectedBody = true;
            }
        };

        var colorBodyDialog = new ColorPickerDialog();
        colorBodyDialog.ButtonOk.Click += (sender, args) =>
        {
            simulationData.NewBodyColor = colorBodyDialog.Color;
            simulationData.ColorSelectedBody = true;
        };
        
        var colorBodyButton = UiComponents.Button("Change Body Colour", false);
        colorBodyButton.Click += (sender, args) =>
        {
            if (simulationData.EditMode && simulationData.IsABodySelected)
            {
                colorBodyDialog.Show(desktop);
            }
        };
        
        var editBodyDialog = EditBodyDialog.Create(simulationData, desktop);
        var editBodyButton = UiComponents.Button("Edit Body Properties", false);
        editBodyButton.Click += (sender, args) =>
        {
            if (simulationData.EditMode && simulationData.IsABodySelected)
            {
                PopulateFormData(editBodyDialog, simulationData);
                editBodyDialog.Show(desktop);
            }
        };
        
        var editModeButton = UiComponents.Button("Enter Edit Mode");
        editModeButton.Click += (sender, args) =>
        {
            ((Label)editModeButton.Content).Text = simulationData.EditMode ? "Enter Edit Mode" : "Exit Edit Mode";
            simulationData.IsPaused = !simulationData.IsPaused;
            simulationData.EditMode = !simulationData.EditMode;
            deleteBodyButton.Visible = simulationData.EditMode;
            colorBodyButton.Visible = simulationData.EditMode;
            editBodyButton.Visible = simulationData.EditMode;
        };
        
        var createBodyDialog = CreateBodyDialog.Create(simulationData, desktop);

        var createBodyButton = UiComponents.Button("Create Body");
        createBodyButton.Click += (s, e) =>
        {
            createBodyDialog.Show(desktop);
        };
        
        var editPanel = 
            UiComponents.VerticalStackPanel(8, HorizontalAlignment.Right, VerticalAlignment.Bottom,
                new Thickness(0, 0, UiConstants.DefaultMargin, UiConstants.DefaultMargin));
        
        editPanel.Widgets.Add(deleteBodyButton);
        editPanel.Widgets.Add(colorBodyButton);
        editPanel.Widgets.Add(editBodyButton);
        editPanel.Widgets.Add(editModeButton);
        editPanel.Widgets.Add(createBodyButton);

        return editPanel;
    }
    
    private static void PopulateFormData(Dialog dialog, SimulationData simulationData)
    {
        ((TextBox)dialog.FindChildById("bodyNameTextbox")).Text = simulationData.SelectedBodyData.Name;
        ((TextBox)dialog.FindChildById("bodyPosXTextbox")).Text =
            simulationData.SelectedBodyData.Position.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyPosYTextbox")).Text =
            simulationData.SelectedBodyData.Position.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelXTextbox")).Text =
            simulationData.SelectedBodyData.Velocity.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelYTextbox")).Text =
            simulationData.SelectedBodyData.Velocity.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyMassTextbox")).Text =
            simulationData.SelectedBodyData.Mass.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyDisplaySizeTextbox")).Text =
            simulationData.SelectedBodyData.DisplayRadius.ToString(CultureInfo.CurrentCulture);
    }
}