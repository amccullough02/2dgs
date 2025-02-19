using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class SettingsPanel
{
    private const int MinTimeStep = 1;
    private const int MaxTimeStep = 500;

    public static VerticalStackPanel Create(SimulationMediator simulationMediator)
    {
        var settingsPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(UiConstants.DefaultMargin, 0, 0, UiConstants.DefaultMargin),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom
        };

        var timeStepLabel = UiComponents.LightLabel($"Time step: {simulationMediator.TimeStep}");
        timeStepLabel.Id = "speed_label";

        var timeStepSlider = UiComponents.HorizontalSlider(simulationMediator.TimeStep, MinTimeStep, MaxTimeStep);
        timeStepSlider.Id = "speed_slider";
        timeStepSlider.ValueChanged += (_, _) =>
        {
            timeStepLabel.Text = $"Time step: {(int)timeStepSlider.Value}";
            simulationMediator.TimeStep = (int)timeStepSlider.Value;
        };

        var pauseButton = UiComponents.Button("Pause Simulation");
        pauseButton.Id = "pause_button";
        pauseButton.Click += (_, _) =>
        {
            ((Label)pauseButton.Content).Text = simulationMediator.Paused ? "Pause Simulation" : "Resume Simulation";
            simulationMediator.Paused = !simulationMediator.Paused;
        };

        var firstDivider = UiComponents.HorizontalSeparator();
        
        var trailLengthLabel = UiComponents.LightLabel($"Trail length: {simulationMediator.TrailLength}");

        var trailLengthSlider = UiComponents.HorizontalSlider(250, 250, 2000);
        trailLengthSlider.ValueChanged += (_, _) =>
        {
            trailLengthLabel.Text = $"Trail length: {(int)trailLengthSlider.Value}";
            simulationMediator.TrailLength = (int)trailLengthSlider.Value;
        };
        
        var trailsButton = UiComponents.Button("Toggle Trails");
        trailsButton.Click += (_, _) =>
        {
            simulationMediator.ToggleTrails = !simulationMediator.ToggleTrails;
        };
        
        var secondDivider = UiComponents.HorizontalSeparator();
        
        var namesButton = UiComponents.Button("Toggle Names");
        namesButton.Click += (_, _) =>
        {
            simulationMediator.ToggleNames = !simulationMediator.ToggleNames;
        };

        var namesDropdown = UiComponents.ComboView();
        namesDropdown.Id = "name_position";
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Right"));
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Left"));
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Top"));
        namesDropdown.Widgets.Add(UiComponents.DropdownLabel("Bottom"));
        namesDropdown.SelectedIndex = 0;
        namesDropdown.SelectedIndexChanged += (_, _) =>
        {
            simulationMediator.Position = namesDropdown.SelectedIndex switch
            {
                0 => Position.Right,
                1 => Position.Left,
                2 => Position.Top,
                3 => Position.Bottom,
                _ => simulationMediator.Position
            };
        };
        
        var thirdDivider = UiComponents.HorizontalSeparator();
        var glowButton = UiComponents.Button("Toggle Glow");
        glowButton.Click += (_, _) =>
        {
            simulationMediator.ToggleGlow = !simulationMediator.ToggleGlow;
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