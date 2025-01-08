using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationUI
{
    private Game game;
    private FontManager fontManager;
    private bool wasKeyPreviouslyDown;
    
    public SimulationUI(Game game)
    {
        this.game = game;
        fontManager = new FontManager();
    }

    public VerticalStackPanel SettingsPanel(SimulationData simData)
    {
        var settingsPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(20, 0, 0, 20),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        var timestepLabel = new Label
        {
            Text = $"Time step: {simData.TimeStep}",
            Font = fontManager.LightFont(20)
        };

        var timestepSlider = new HorizontalSlider
        {
            Minimum = 1,
            Maximum = 10,
            Value = 1,
            Width = 250,
        };

        timestepSlider.ValueChanged += (s, e) =>
        {
            timestepLabel.Text = $"Time step: {(int)timestepSlider.Value}";
            simData.TimeStep = (int)timestepSlider.Value;
        };
        
        var pauseButton = new Button()
        {
            Width = 250,
            Height = 60,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Pause Simulation",
                Font = fontManager.LightFont(20)
            }
        };

        pauseButton.Click += (s, e) =>
        {
            simData.IsPaused = !simData.IsPaused;
        };

        var firstDivider = new HorizontalSeparator
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Thickness = 5,
            Color = Color.White,
            Width = 250,
            Margin = new Thickness(0, 10, 0, 10),
        };
        
        var trailLengthLabel = new Label
        {
            Text = $"Trail length: {simData.TrailLength}",
            Font = fontManager.LightFont(20)
        };

        var trailLengthSlider = new HorizontalSlider
        {
            Minimum = 250,
            Maximum = 2000,
            Value = 1,
            Width = 250,
        };

        trailLengthSlider.ValueChanged += (s, e) =>
        {
            trailLengthLabel.Text = $"Trail length: {(int)trailLengthSlider.Value}";
            simData.TrailLength = (int)trailLengthSlider.Value;
        };
        
        var trailsButton = new Button()
        {
            Width = 250,
            Height = 60,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Toggle Trails",
                Font = fontManager.LightFont(20)
            }
        };

        trailsButton.Click += (s, e) =>
        {
            simData.ToggleTrails = !simData.ToggleTrails;
        };
        
        var secondDivider = new HorizontalSeparator
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Thickness = 5,
            Color = Color.White,
            Width = 250,
            Margin = new Thickness(0, 10, 0, 10),
        };
        
        var namesButton = new Button()
        {
            Width = 250,
            Height = 60,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Toggle Names",
                Font = fontManager.LightFont(20)
            }
        };

        namesButton.Click += (s, e) =>
        {
            simData.ToggleNames = !simData.ToggleNames;
        };

        var namesDropdown = new ComboView()
        {
            Width = 250,
            SelectedIndex = 0,
        };
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Left",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Right",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Top",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Bottom",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });

        namesDropdown.SelectedIndex = 0;
        namesDropdown.SelectedIndexChanged += (s, e) =>
        {
            switch (namesDropdown.SelectedIndex)
            {
                case 0:
                    simData.Position = Position.Left;
                    break;
                case 1:
                    simData.Position = Position.Right;
                    break;
                case 2:
                    simData.Position = Position.Top;
                    break;
                case 3:
                    simData.Position = Position.Bottom;
                    break;
            }
        };
        
        settingsPanel.Widgets.Add(timestepLabel);
        settingsPanel.Widgets.Add(timestepSlider);
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

    public Button ReturnButton ()
    {
        Button returnButton = new Button
        {
            Width = 250,
            Height = 60,
            Margin = new Thickness(0, 20, 20, 0),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Simulation Menu",
                Font = fontManager.LightFont(20)
            }
        };
        
        returnButton.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        return returnButton;
    }

    public void PauseToggle(SimulationData simData)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        }

        var keyboardState = Keyboard.GetState();
        bool isKeyDown = keyboardState.IsKeyDown(Keys.P);
        if (isKeyDown && !wasKeyPreviouslyDown)
        {
            simData.IsPaused = !simData.IsPaused;
            Console.WriteLine($"DEBUG: Paused: {simData.IsPaused}");
        }
        wasKeyPreviouslyDown = isKeyDown;
    }
}