using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationUi
{
    private Desktop _desktop;
    
    public SimulationUi(Game game, SimulationData simulationData)
    {
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(SettingsPanel(simulationData));
        rootContainer.Widgets.Add(EditPanel(simulationData));
        rootContainer.Widgets.Add(ReturnPanel(game));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel SettingsPanel(SimulationData simulationData)
    {
        var settingsPanel =
            UiComponents.CreateVerticalStackPanel(8, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                new Thickness(UiConstants.DefaultMargin, 0, 0, UiConstants.DefaultMargin));

        var timeStepLabel = UiComponents.CreateStyledLabel($"Time step: {simulationData.TimeStep}");

        var timeStepSlider = UiComponents.CreateHorizontalSlider(1, 1, 10);
        timeStepSlider.ValueChanged += (s, e) =>
        {
            timeStepLabel.Text = $"Time step: {(int)timeStepSlider.Value}";
            simulationData.TimeStep = (int)timeStepSlider.Value;
        };

        var pauseButton = UiComponents.CreateButton("Pause Simulation");
        pauseButton.Click += (s, e) =>
        {
            simulationData.IsPaused = !simulationData.IsPaused;
            if (simulationData.EditMode) simulationData.EditMode = false;
        };

        var firstDivider = UiComponents.CreateHorizontalSeparator();
        
        var trailLengthLabel = UiComponents.CreateStyledLabel($"Trail length: {simulationData.TrailLength}");

        var trailLengthSlider = UiComponents.CreateHorizontalSlider(250, 250, 2000);
        trailLengthSlider.ValueChanged += (s, e) =>
        {
            trailLengthLabel.Text = $"Trail length: {(int)trailLengthSlider.Value}";
            simulationData.TrailLength = (int)trailLengthSlider.Value;
        };
        
        var trailsButton = UiComponents.CreateButton("Toggle Trails");
        trailsButton.Click += (s, e) =>
        {
            simulationData.ToggleTrails = !simulationData.ToggleTrails;
        };
        
        var secondDivider = UiComponents.CreateHorizontalSeparator();
        
        var namesButton = UiComponents.CreateButton("Toggle Names");
        namesButton.Click += (s, e) =>
        {
            simulationData.ToggleNames = !simulationData.ToggleNames;
        };

        var namesDropdown = UiComponents.CreateComboView();
        namesDropdown.Widgets.Add(UiComponents.CreateDropdownLabel("Left"));
        namesDropdown.Widgets.Add(UiComponents.CreateDropdownLabel("Right"));
        namesDropdown.Widgets.Add(UiComponents.CreateDropdownLabel("Top"));
        namesDropdown.Widgets.Add(UiComponents.CreateDropdownLabel("Bottom"));
        namesDropdown.SelectedIndex = 0;
        namesDropdown.SelectedIndexChanged += (s, e) =>
        {
            switch (namesDropdown.SelectedIndex)
            {
                case 0:
                    simulationData.Position = Position.Left;
                    break;
                case 1:
                    simulationData.Position = Position.Right;
                    break;
                case 2:
                    simulationData.Position = Position.Top;
                    break;
                case 3:
                    simulationData.Position = Position.Bottom;
                    break;
            }
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

        return settingsPanel;
    }

    private Dialog CreateBodyDialog(SimulationData simulationData)
    {
        var grid = UiComponents.CreateGrid(10);
        UiComponents.AddGridColumns(grid, 2);
        UiComponents.AddGridRows(grid, 5);
        
        var bodyNameLabel = UiComponents.CreateDialogLabel("Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.CreateBasicTextBox("Default name");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        
        var bodyVelXLabel = UiComponents.CreateDialogLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 1);
        var bodyVelXTextbox = UiComponents.CreateBasicTextBox("0.0");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 1);
        
        var bodyVelYLabel = UiComponents.CreateDialogLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 2);
        var bodyVelYTextbox = UiComponents.CreateBasicTextBox("4.0");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 2);
        
        var bodyMassLabel = UiComponents.CreateDialogLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 3);
        var bodyMassTextbox = UiComponents.CreateBasicTextBox("1e6");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 3);

        var bodyDisplaySizeLabel = UiComponents.CreateDialogLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 4);
        var bodyDisplaySizeTextbox = UiComponents.CreateBasicTextBox("0.05");
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 4);
        
        var createBodyDialogue = new Dialog { Title = "Create New Body", Content = grid };

        createBodyDialogue.ButtonOk.Click += (sender, e) =>
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
        };

        return createBodyDialogue;
    }

    private Dialog EditBodyDialog(SimulationData simulationData)
    {
        var grid = UiComponents.CreateGrid(10);
        UiComponents.AddGridColumns(grid, 2);
        UiComponents.AddGridRows(grid, 7);
        
        var bodyNameLabel = UiComponents.CreateDialogLabel("Name: ");
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);
        var bodyNameTextbox = UiComponents.CreateBasicTextBox("Default name");
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);

        var bodyPosXLabel = UiComponents.CreateDialogLabel("Pos X: ");
        grid.Widgets.Add(bodyPosXLabel);
        Grid.SetRow(bodyPosXLabel, 1);
        var posXTextbox = UiComponents.CreateBasicTextBox("0.0");
        grid.Widgets.Add(posXTextbox);
        Grid.SetColumn(posXTextbox, 1);
        Grid.SetRow(posXTextbox, 1);

        var bodyPosYLabel = UiComponents.CreateDialogLabel("Pos Y: ");
        grid.Widgets.Add(bodyPosYLabel);
        Grid.SetRow(bodyPosYLabel, 2);
        var posYTextbox = UiComponents.CreateBasicTextBox("0.0");
        grid.Widgets.Add(posYTextbox);
        Grid.SetColumn(posYTextbox, 1);
        Grid.SetRow(posYTextbox, 2);

        var bodyVelXLabel = UiComponents.CreateDialogLabel("Vel X: ");
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 3);
        var bodyVelXTextbox = UiComponents.CreateBasicTextBox("0.0");
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 3);

        var bodyVelYLabel = UiComponents.CreateDialogLabel("Vel Y: ");
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 4);
        var bodyVelYTextbox = UiComponents.CreateBasicTextBox("4.0");
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 4);

        var bodyMassLabel = UiComponents.CreateDialogLabel("Mass: ");
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 5);
        var bodyMassTextbox = UiComponents.CreateBasicTextBox("1e6");
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 5);

        var bodyDisplaySizeLabel = UiComponents.CreateDialogLabel("Display Size: ");
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 6);
        var bodyDisplaySizeTextbox = UiComponents.CreateBasicTextBox("0.05");
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 6);

    var editBodyDialog = new Dialog { Title = "Edit Body", Content = grid };

        editBodyDialog.ButtonOk.Click += (sender, e) =>
        {
            string name = bodyNameTextbox.Text;
            Vector2 position = new Vector2(float.Parse(posXTextbox.Text), float.Parse(posYTextbox.Text));
            Vector2 velocity = new Vector2(float.Parse(bodyVelXTextbox.Text), float.Parse(bodyVelYTextbox.Text));
            float mass = float.Parse(bodyMassTextbox.Text);
            float size = float.Parse(bodyDisplaySizeTextbox.Text);
            
            simulationData.EditBodyData.Name = name;
            simulationData.EditBodyData.Position = position;
            simulationData.EditBodyData.Velocity = velocity;
            simulationData.EditBodyData.Mass = mass;
            simulationData.EditBodyData.DisplayRadius = size;
            simulationData.EditSelectedBody = true;
        };

        return editBodyDialog;
    }

    private VerticalStackPanel EditPanel(SimulationData simulationData)
    {
        var createBodyDialogue = CreateBodyDialog(simulationData);

        var createBodyButton = UiComponents.CreateButton("Create Body");
        createBodyButton.Click += (s, e) =>
        {
            createBodyDialogue.Show(_desktop);
        };
        
        var editBodyDialog = EditBodyDialog(simulationData);

        var editModeButton = UiComponents.CreateButton("Edit Mode");
        editModeButton.Click += (sender, args) =>
        {
            simulationData.IsPaused = !simulationData.IsPaused;
            simulationData.EditMode = !simulationData.EditMode;
        };
        
        var editBodyButton = UiComponents.CreateButton("Edit Body");
        editBodyButton.Click += (sender, args) =>
        {
            if (simulationData.EditMode && simulationData.IsABodySelected)
            {
                editBodyDialog.Show(_desktop);
            }
        };

        var deleteBodyButton = UiComponents.CreateButton("Delete Body");
        deleteBodyButton.Click += (sender, args) =>
        {
            if (simulationData.EditMode && simulationData.IsABodySelected)
            {
                simulationData.DeleteSelectedBody = true;
            }
        };

        var editPanel = 
            UiComponents.CreateVerticalStackPanel(8, HorizontalAlignment.Right, VerticalAlignment.Bottom,
                new Thickness(0, 0, UiConstants.DefaultMargin, UiConstants.DefaultMargin));
        
        editPanel.Widgets.Add(deleteBodyButton);
        editPanel.Widgets.Add(editBodyButton);
        editPanel.Widgets.Add(editModeButton);
        editPanel.Widgets.Add(createBodyButton);

        return editPanel;
    }

    private VerticalStackPanel ReturnPanel(Game game)
    {
        var returnPanel = UiComponents.CreateVerticalStackPanel(8, HorizontalAlignment.Right, VerticalAlignment.Top,
            new Thickness(0, UiConstants.DefaultMargin, UiConstants.DefaultMargin, 0));
        
        var returnButton = UiComponents.CreateButton("Exit Simulation");
        returnButton.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        returnPanel.Widgets.Add(returnButton);
        
        return returnPanel;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}