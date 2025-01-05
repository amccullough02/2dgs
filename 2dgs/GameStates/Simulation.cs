﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class Simulation : GameState
{
    private Game game;
    private string filePath;
    private List<Body> bodies;
    private SaveSystem saveSystem;
    private bool isPaused;
    
    public Simulation(Game game, string filePath)
    {
        this.game = game;
        this.filePath = filePath;
        
        bodies = new List<Body>();
        saveSystem = new SaveSystem();
        
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
                body.Update(bodies);
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
    }
}