﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    private FontManager fontManager;
    private bool isPaused;
    private int timeStep = 1;
    
    public Simulation(Game game, string filePath)
    {
        this.game = game;

        bodies = new List<Body>();
        saveSystem = new SaveSystem();
        fontManager = new FontManager();
        
        SaveData saveData = saveSystem.Load(filePath);

        if (saveData?.Bodies != null)
        {
            foreach (var bodyData in saveData.Bodies)
            {
                bodies.Add(new Body(bodyData.Position, bodyData.Mass, bodyData.DisplayRadius));
            }
        }
        
        foreach (Body body in bodies)
        {
            body.LoadContent(game.Content);
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
            Text = $"Time step: {timeStep}",
            Font = fontManager.GetOrbitronLightFont(20)
        };

        var timestepSlider = new HorizontalSlider
        {
            Minimum = 1,
            Maximum = 10,
            Value = 1,
            Width = 200,
        };

        timestepSlider.ValueChanged += (s, e) =>
        {
            timestepLabel.Text = $"Time step: {(int)timestepSlider.Value}";
            timeStep = (int)timestepSlider.Value;
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
            isPaused = !isPaused;
        };
        
        verticalPane.Widgets.Add(timestepLabel);
        verticalPane.Widgets.Add(timestepSlider);
        verticalPane.Widgets.Add(pauseButton);

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
            isPaused = !isPaused;
            Console.WriteLine($"DEBUG: Paused: {isPaused}");
        }
        wasKeyPreviouslyDown = isKeyDown;
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!isPaused)
        {
            foreach (Body body in bodies)
            {
                body.Update(bodies, timeStep);
            }
        }
        
        PauseToggle();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        foreach (Body body in bodies)
        {
            body.Draw(spriteBatch);
        }
        
        spriteBatch.End();
        
        desktop.Render();
    }
}