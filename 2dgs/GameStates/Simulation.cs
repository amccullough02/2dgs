using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class Simulation : GameState
{
    private Desktop desktop;
    private Game game;
    private List<Body> bodies;
    private SaveSystem saveSystem;
    private SaveData saveData;
    private FontManager fontManager;
    private SimulationData simData;
    
    public Simulation(Game game, string filePath)
    {
        this.game = game;
        simData = new SimulationData();
        bodies = new List<Body>();
        saveSystem = new SaveSystem();
        fontManager = new FontManager();
        
        saveData = saveSystem.Load(filePath);

        if (saveData?.Bodies != null)
        {
            foreach (var bodyData in saveData.Bodies)
            {
                bodies.Add(new Body(bodyData.Name, bodyData.Position, bodyData.Mass, bodyData.DisplayRadius));
            }
        }
        
        foreach (Body body in bodies)
        {
            body.LoadContent(game.Content, game.GraphicsDevice);
        }
        
        Console.WriteLine($"DEBUG: Simulation loaded: {filePath}");
        
        MyraEnvironment.Game = game;

        var verticalPane = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(20, 0, 0, 20),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        var timestepLabel = new Label
        {
            Text = $"Time step: {simData.TimeStep}",
            Font = fontManager.GetOrbitronLightFont(20)
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
                Font = fontManager.GetOrbitronLightFont(20)
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
            Font = fontManager.GetOrbitronLightFont(20)
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
                Font = fontManager.GetOrbitronLightFont(20)
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
                Font = fontManager.GetOrbitronLightFont(20)
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
            Font = fontManager.GetOrbitronLightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Right",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.GetOrbitronLightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Top",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.GetOrbitronLightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Bottom",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = fontManager.GetOrbitronLightFont(18),
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
        
        verticalPane.Widgets.Add(timestepLabel);
        verticalPane.Widgets.Add(timestepSlider);
        verticalPane.Widgets.Add(pauseButton);
        verticalPane.Widgets.Add(firstDivider);
        verticalPane.Widgets.Add(trailLengthLabel);
        verticalPane.Widgets.Add(trailLengthSlider);
        verticalPane.Widgets.Add(trailsButton);
        verticalPane.Widgets.Add(secondDivider);
        verticalPane.Widgets.Add(namesButton);
        verticalPane.Widgets.Add(namesDropdown);

        var returnButton = new Button
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
                Font = fontManager.GetOrbitronLightFont(20)
            }
        };
        
        returnButton.Click += (s, e) =>
        {
            game.GameStateManager.ChangeState(new SimulationMenu(game));
        };
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(verticalPane);
        rootContainer.Widgets.Add(returnButton);
        
        desktop = new Desktop();
        desktop.Root = rootContainer;
        
        TestSimulationLoading();
    }
    
    private bool wasKeyPreviouslyDown = false;

    private void PauseToggle()
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

    private void TestSimulationLoading()
    {
        int serializedBodiesCount = saveData.Bodies.Count;
        int loadedBodiesCount = bodies.Count;
        
        if (serializedBodiesCount == loadedBodiesCount)
        {
            Console.WriteLine("Test - Loading of simulation file... PASS!");
        }
        else
        {
            Console.WriteLine("Test - Loading of simulation file... FAIL!");
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!simData.IsPaused)
        {
            foreach (Body body in bodies)
            {
                body.Update(bodies, simData.TimeStep);
            }
        }
        
        PauseToggle();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        foreach (Body body in bodies)
        {
            body.Draw(spriteBatch, simData.ToggleTrails, simData.TrailLength, simData.ToggleNames, simData.Position);
        }
        
        spriteBatch.End();
        
        desktop.Render();
    }
}