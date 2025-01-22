using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class SettingsPanel
{
    public static VerticalStackPanel Create(SimulationData simulationData)
    {
        var settingsPanel =
            UiComponents.VerticalStackPanel(8, HorizontalAlignment.Left, VerticalAlignment.Bottom,
                new Thickness(UiConstants.DefaultMargin, 0, 0, UiConstants.DefaultMargin));

        var timeStepLabel = UiComponents.Label($"Time step: {simulationData.TimeStep}");

        var timeStepSlider = UiComponents.HorizontalSlider(simulationData.TimeStep, 10, 400);
        timeStepSlider.Id = "speed_slider";
        timeStepSlider.ValueChanged += (s, e) =>
        {
            timeStepLabel.Text = $"Time step: {(int)timeStepSlider.Value}";
            simulationData.TimeStep = (int)timeStepSlider.Value;
        };

        var pauseButton = UiComponents.Button("Pause Simulation");
        pauseButton.Id = "pause_button";
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
        namesDropdown.Id = "name_position";
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
}