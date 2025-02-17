using System.Globalization;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace _2dgs;

public static class EditModePanel
{
    public static VerticalStackPanel Create(SimulationMediator simulationMediator, Desktop desktop)
    {
        var deleteBodyButton = UiComponents.Button("Delete Body", false);
        deleteBodyButton.Id = "delete_body_button";
        deleteBodyButton.Click += (sender, args) =>
        {
            if (simulationMediator.EditMode && simulationMediator.ABodySelected)
            {
                simulationMediator.DeleteSelectedBody = true;
            }
        };

        var colorBodyDialog = new ColorPickerDialog();
        colorBodyDialog.ButtonOk.Click += (sender, args) =>
        {
            simulationMediator.NewBodyColor = colorBodyDialog.Color;
            simulationMediator.ColorSelectedBody = true;
        };
        
        var colorBodyButton = UiComponents.Button("Change Body Colour", false);
        colorBodyButton.Id = "body_color_button";
        colorBodyButton.Click += (sender, args) =>
        {
            if (simulationMediator.EditMode && simulationMediator.ABodySelected)
            {
                colorBodyDialog.Show(desktop);
            }
        };
        
        var editBodyDialog = EditBodyDialog.Create(simulationMediator, desktop);
        var editBodyButton = UiComponents.Button("Edit Body Properties", false);
        editBodyButton.Id = "edit_body_button";
        editBodyButton.Click += (sender, args) =>
        {
            if (simulationMediator.EditMode && simulationMediator.ABodySelected)
            {
                PopulateFormData(editBodyDialog, simulationMediator);
                editBodyDialog.Show(desktop);
            }
        };
        
        var editModeButton = UiComponents.Button("Enter Edit Mode");
        editModeButton.Id = "edit_mode";
        editModeButton.Click += (sender, args) =>
        {
            ((Label)editModeButton.Content).Text = simulationMediator.EditMode ? "Enter Edit Mode" : "Exit Edit Mode";
            simulationMediator.EditMode = !simulationMediator.EditMode;
            deleteBodyButton.Visible = simulationMediator.EditMode;
            colorBodyButton.Visible = simulationMediator.EditMode;
            editBodyButton.Visible = simulationMediator.EditMode;
        };
        
        var createBodyDialog = CreateBodyDialog.Create(simulationMediator, desktop);

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
    
    private static void PopulateFormData(Dialog dialog, SimulationMediator simulationMediator)
    {
        ((TextBox)dialog.FindChildById("bodyNameTextbox")).Text = simulationMediator.SelectedBodyData.Name;
        ((TextBox)dialog.FindChildById("bodyPosXTextbox")).Text =
            simulationMediator.SelectedBodyData.Position.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyPosYTextbox")).Text =
            simulationMediator.SelectedBodyData.Position.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelXTextbox")).Text =
            simulationMediator.SelectedBodyData.Velocity.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelYTextbox")).Text =
            simulationMediator.SelectedBodyData.Velocity.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyMassTextbox")).Text =
            simulationMediator.SelectedBodyData.Mass.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyDisplaySizeTextbox")).Text =
            simulationMediator.SelectedBodyData.DisplaySize.ToString(CultureInfo.CurrentCulture);
    }
}