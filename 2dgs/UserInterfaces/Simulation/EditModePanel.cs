using System.Globalization;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace _2dgs;

public static class EditModePanel
{
    public static VerticalStackPanel Create(SimulationSceneData simulationSceneData, Desktop desktop)
    {
        var deleteBodyButton = UiComponents.Button("Delete Body", false);
        deleteBodyButton.Id = "delete_body_button";
        deleteBodyButton.Click += (sender, args) =>
        {
            if (simulationSceneData.EditMode && simulationSceneData.ABodySelected)
            {
                simulationSceneData.DeleteSelectedBody = true;
            }
        };

        var colorBodyDialog = new ColorPickerDialog();
        colorBodyDialog.ButtonOk.Click += (sender, args) =>
        {
            simulationSceneData.NewBodyColor = colorBodyDialog.Color;
            simulationSceneData.ColorSelectedBody = true;
        };
        
        var colorBodyButton = UiComponents.Button("Change Body Colour", false);
        colorBodyButton.Id = "body_color_button";
        colorBodyButton.Click += (sender, args) =>
        {
            if (simulationSceneData.EditMode && simulationSceneData.ABodySelected)
            {
                colorBodyDialog.Show(desktop);
            }
        };
        
        var editBodyDialog = EditBodyDialog.Create(simulationSceneData, desktop);
        var editBodyButton = UiComponents.Button("Edit Body Properties", false);
        editBodyButton.Id = "edit_body_button";
        editBodyButton.Click += (sender, args) =>
        {
            if (simulationSceneData.EditMode && simulationSceneData.ABodySelected)
            {
                PopulateFormData(editBodyDialog, simulationSceneData);
                editBodyDialog.Show(desktop);
            }
        };
        
        var editModeButton = UiComponents.Button("Enter Edit Mode");
        editModeButton.Id = "edit_mode";
        editModeButton.Click += (sender, args) =>
        {
            ((Label)editModeButton.Content).Text = simulationSceneData.EditMode ? "Enter Edit Mode" : "Exit Edit Mode";
            simulationSceneData.EditMode = !simulationSceneData.EditMode;
            deleteBodyButton.Visible = simulationSceneData.EditMode;
            colorBodyButton.Visible = simulationSceneData.EditMode;
            editBodyButton.Visible = simulationSceneData.EditMode;
        };
        
        var createBodyDialog = CreateBodyDialog.Create(simulationSceneData, desktop);

        var createBodyButton = UiComponents.Button("Create Body");
        createBodyButton.Id = "create_body";
        createBodyButton.Click += (s, e) =>
        {
            createBodyDialog.Show(desktop);
        };

        var editPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(0, 0, UiConstants.DefaultMargin, UiConstants.DefaultMargin),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
        
        editPanel.Widgets.Add(deleteBodyButton);
        editPanel.Widgets.Add(colorBodyButton);
        editPanel.Widgets.Add(editBodyButton);
        editPanel.Widgets.Add(editModeButton);
        editPanel.Widgets.Add(createBodyButton);

        return editPanel;
    }
    
    private static void PopulateFormData(Dialog dialog, SimulationSceneData simulationSceneData)
    {
        ((TextBox)dialog.FindChildById("bodyNameTextbox")).Text = simulationSceneData.SelectedBodyData.Name;
        ((TextBox)dialog.FindChildById("bodyPosXTextbox")).Text =
            simulationSceneData.SelectedBodyData.Position.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyPosYTextbox")).Text =
            simulationSceneData.SelectedBodyData.Position.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelXTextbox")).Text =
            simulationSceneData.SelectedBodyData.Velocity.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelYTextbox")).Text =
            simulationSceneData.SelectedBodyData.Velocity.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyMassTextbox")).Text =
            simulationSceneData.SelectedBodyData.Mass.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyDisplaySizeTextbox")).Text =
            simulationSceneData.SelectedBodyData.DisplaySize.ToString(CultureInfo.CurrentCulture);
    }
}