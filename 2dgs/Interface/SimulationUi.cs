using System.Globalization;
using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace _2dgs;

public class SimulationUi
{
    private readonly Desktop _desktop;
    
    public SimulationUi(Game game, SimulationData simulationData)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(SettingsPanel(simulationData));
        rootContainer.Widgets.Add(EditPanel(simulationData));
        rootContainer.Widgets.Add(SaveAndQuitPanel(game, simulationData));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel SettingsPanel(SimulationData simulationData)
    {
        var settingsPanel =
            UiComponents.VerticalStackPanel(8, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                new Thickness(UiConstants.DefaultMargin, 0, 0, UiConstants.DefaultMargin));

        var timeStepLabel = UiComponents.Label($"Time step: {simulationData.TimeStep}");

        var timeStepSlider = UiComponents.HorizontalSlider(simulationData.TimeStep, 10, 400);
        timeStepSlider.ValueChanged += (s, e) =>
        {
            timeStepLabel.Text = $"Time step: {(int)timeStepSlider.Value}";
            simulationData.TimeStep = (int)timeStepSlider.Value;
        };

        var pauseButton = UiComponents.Button("Pause Simulation");
        pauseButton.Click += (s, e) =>
        {
            if (!simulationData.EditMode)
            {
                ((Label)pauseButton.Content).Text = simulationData.IsPaused ? "Pause Simulation" : "Resume Simulation";
                simulationData.IsPaused = !simulationData.IsPaused;
                if (simulationData.EditMode) simulationData.EditMode = false;
            }
        };

        var firstDivider = UiComponents.HorizontalSeparator();
        
        var trailLengthLabel = UiComponents.Label($"Trail length: {simulationData.TrailLength}");

        var trailLengthSlider = UiComponents.HorizontalSlider(250, 250, 2000);
        trailLengthSlider.ValueChanged += (s, e) =>
        {
            trailLengthLabel.Text = $"Trail length: {(int)trailLengthSlider.Value}";
            simulationData.TrailLength = (int)trailLengthSlider.Value;
        };
        
        var trailsButton = UiComponents.Button("Toggle Trails");
        trailsButton.Click += (s, e) =>
        {
            simulationData.ToggleTrails = !simulationData.ToggleTrails;
        };
        
        var secondDivider = UiComponents.HorizontalSeparator();
        
        var namesButton = UiComponents.Button("Toggle Names");
        namesButton.Click += (s, e) =>
        {
            simulationData.ToggleNames = !simulationData.ToggleNames;
        };

        var namesDropdown = UiComponents.ComboView();
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Right"));
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Left"));
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Top"));
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Bottom"));
        namesDropdown.SelectedIndex = 0;
        namesDropdown.SelectedIndexChanged += (s, e) =>
        {
            switch (namesDropdown.SelectedIndex)
            {
                case 0:
                    simulationData.Position = Position.Right;
                    break;
                case 1:
                    simulationData.Position = Position.Left;
                    break;
                case 2:
                    simulationData.Position = Position.Top;
                    break;
                case 3:
                    simulationData.Position = Position.Bottom;
                    break;
            }
        };
        
        var thirdDivider = UiComponents.HorizontalSeparator();
        var glowButton = UiComponents.Button("Toggle Glow");
        glowButton.Click += (s, e) =>
        {
            simulationData.ToggleGlow = !simulationData.ToggleGlow;
        };
        
        settingsPanel.Widgets.Add(timeStepLabel);
        settingsPanel.Widgets.Add(timeStepSlider);
        settingsPanel.Widgets.Add(pauseButton);
        settingsPanel.Widgets.Add(firstDivider);
        settingsPanel.Widgets.Add(trailLengthLabel);
        settingsPanel.Widgets.Add(trailLengthSlider);
        settingsPanel.Widgets.Add(trailsButton);
        settingsPanel.Widgets.Add(secondDivider);
        settingsPanel.Widgets.Add(namesButton);
        settingsPanel.Widgets.Add(namesDropdown);
        settingsPanel.Widgets.Add(thirdDivider);
        settingsPanel.Widgets.Add(glowButton);

        return settingsPanel;
    }

    private Dialog CreateBodyDialog(SimulationData simulationData)
    {
        var grid = UiComponents.Grid(10, 2, 5);
        
        var bodyNameLabel = UiComponents.DialogLabel("Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.TextBox("Default name");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        
        var bodyVelXLabel = UiComponents.DialogLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 1);
        var bodyVelXTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 1);
        
        var bodyVelYLabel = UiComponents.DialogLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 2);
        var bodyVelYTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 2);
        
        var bodyMassLabel = UiComponents.DialogLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 3);
        var bodyMassTextbox = UiComponents.TextBox("1e6");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 3);

        var bodyDisplaySizeLabel = UiComponents.DialogLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 4);
        var bodyDisplaySizeTextbox = UiComponents.TextBox("0.05");
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 4);

        var createBodyDialog = UiComponents.StyledDialog("Create New Body");
        createBodyDialog.Content = grid;
        
        var validationErrorMessage = UiComponents.DialogLabel("Validation Error: ");
        var validationErrorDialogue = UiComponents.ValidationWindow(validationErrorMessage);
        validationErrorDialogue.CloseButton.Click += (s, e) => { createBodyDialog.Show(_desktop); };

        createBodyDialog.ButtonOk.Click += (sender, e) =>
        {
            bool valid = true;
            string errorMessage = "";

            if (bodyNameTextbox.Text.Length <= 2)
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
                validationErrorDialogue.Show(_desktop);
            }
        };

        return createBodyDialog;
    }

    private Dialog EditBodyDialog(SimulationData simulationData)
    {
        var grid = UiComponents.Grid(10, 2, 7);
        
        var bodyNameLabel = UiComponents.DialogLabel("Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.TextBox("Default");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        bodyNameTextbox.Id = "bodyNameTextbox";

        var bodyPosXLabel = UiComponents.DialogLabel("Pos X: ");
        grid.Widgets.Add(bodyPosXLabel);
        Grid.SetRow(bodyPosXLabel, 1);
        var bodyPosXTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyPosXTextbox);
        Grid.SetColumn(bodyPosXTextbox, 1);
        Grid.SetRow(bodyPosXTextbox, 1);
        bodyPosXTextbox.Id = "bodyPosXTextbox";

        var bodyPosYLabel = UiComponents.DialogLabel("Pos Y: ");
        grid.Widgets.Add(bodyPosYLabel);
        Grid.SetRow(bodyPosYLabel, 2);
        var bodyPosYTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyPosYTextbox);
        Grid.SetColumn(bodyPosYTextbox, 1);
        Grid.SetRow(bodyPosYTextbox, 2);
        bodyPosYTextbox.Id = "bodyPosYTextbox";

        var bodyVelXLabel = UiComponents.DialogLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 3);
        var bodyVelXTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 3);
        bodyVelXTextbox.Id = "bodyVelXTextbox";

        var bodyVelYLabel = UiComponents.DialogLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 4);
        var bodyVelYTextbox = UiComponents.TextBox("0.0");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 4);
        bodyVelYTextbox.Id = "bodyVelYTextbox";

        var bodyMassLabel = UiComponents.DialogLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 5);
        var bodyMassTextbox = UiComponents.TextBox("1e6");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 5);
        bodyMassTextbox.Id = "bodyMassTextbox";

        var bodyDisplaySizeLabel = UiComponents.DialogLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 6);
        var bodyDisplaySizeTextbox = UiComponents.TextBox("0.05");
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 6);
        bodyDisplaySizeTextbox.Id = "bodyDisplaySizeTextbox";

        var editBodyDialog = UiComponents.StyledDialog("Edit Body");
        editBodyDialog.Content = grid;
        var validationErrorMessage = UiComponents.DialogLabel("Validation Error: ");
        var validationErrorDialogue = UiComponents.ValidationWindow(validationErrorMessage);
        validationErrorDialogue.CloseButton.Click += (s, e) => { editBodyDialog.Show(_desktop); };

        editBodyDialog.ButtonOk.Click += (sender, e) =>
        {
            bool valid = true;
            string errorMessage = "";

            if (bodyNameTextbox.Text.Length <= 2)
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

                string name = bodyNameTextbox.Text;
                Vector2 position = new Vector2(float.Parse(bodyPosXTextbox.Text), float.Parse(bodyPosYTextbox.Text));
                Vector2 velocity = new Vector2(float.Parse(bodyVelXTextbox.Text), float.Parse(bodyVelYTextbox.Text));
                float mass = float.Parse(bodyMassTextbox.Text);
                float size = float.Parse(bodyDisplaySizeTextbox.Text);

                simulationData.EditBodyData.Name = name;
                simulationData.EditBodyData.Position = position;
                simulationData.EditBodyData.Velocity = velocity;
                simulationData.EditBodyData.Mass = mass;
                simulationData.EditBodyData.DisplayRadius = size;
                simulationData.EditSelectedBody = true;
                
                editBodyDialog.Close();
            }
            else
            {
                validationErrorMessage.Text = errorMessage;
                validationErrorDialogue.Show(_desktop);
            }
        };

        return editBodyDialog;
    }

    private void PopulateFormData(Dialog dialog, SimulationData simulationData)
    {
        ((TextBox)dialog.FindChildById("bodyNameTextbox")).Text = simulationData.SelectedBodyData.Name;
        ((TextBox)dialog.FindChildById("bodyPosXTextbox")).Text =
            simulationData.SelectedBodyData.Position.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyPosYTextbox")).Text =
            simulationData.SelectedBodyData.Position.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelXTextbox")).Text =
            simulationData.SelectedBodyData.Velocity.X.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyVelXTextbox")).Text =
            simulationData.SelectedBodyData.Velocity.Y.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyMassTextbox")).Text =
            simulationData.SelectedBodyData.Mass.ToString(CultureInfo.CurrentCulture);
        ((TextBox)dialog.FindChildById("bodyDisplaySizeTextbox")).Text =
            simulationData.SelectedBodyData.DisplayRadius.ToString(CultureInfo.CurrentCulture);
    }

    private VerticalStackPanel EditPanel(SimulationData simulationData)
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
                colorBodyDialog.Show(_desktop);
            }
        };
        
        var editBodyDialog = EditBodyDialog(simulationData);
        var editBodyButton = UiComponents.Button("Edit Body Properties", false);
        editBodyButton.Click += (sender, args) =>
        {
            if (simulationData.EditMode && simulationData.IsABodySelected)
            {
                PopulateFormData(editBodyDialog, simulationData);
                editBodyDialog.Show(_desktop);
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
        
        var createBodyDialogue = CreateBodyDialog(simulationData);

        var createBodyButton = UiComponents.Button("Create Body");
        createBodyButton.Click += (s, e) =>
        {
            createBodyDialogue.Show(_desktop);
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

    private VerticalStackPanel SaveAndQuitPanel(Game game, SimulationData simulationData)
    {
        var saveAndQuitPanel = UiComponents.VerticalStackPanel(8, HorizontalAlignment.Right, VerticalAlignment.Top,
            new Thickness(0, UiConstants.DefaultMargin, UiConstants.DefaultMargin, 0));
        
        var returnButton = UiComponents.Button("Exit Simulation");
        returnButton.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        var saveButton = UiComponents.Button("Save Simulation");
        saveButton.Click += (s, e) =>
        {
            simulationData.AttemptToSaveFile = true;
        };
        
        var prompt = new LessonPrompt(simulationData);
        
        UiTests.TestLessonPrompt(simulationData.LessonContent, prompt.GetLessonContent);
        
        var promptButton = UiComponents.Button("Show Lesson Prompt");
        promptButton.Click += (s, e) =>
        {
            prompt.Show(_desktop, simulationData);
        };

        if (!simulationData.IsLesson) promptButton.Visible = false;
        
        saveAndQuitPanel.Widgets.Add(returnButton);
        saveAndQuitPanel.Widgets.Add(saveButton);
        saveAndQuitPanel.Widgets.Add(promptButton);
        
        return saveAndQuitPanel;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}