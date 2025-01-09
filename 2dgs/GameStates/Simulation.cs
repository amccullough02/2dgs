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
    private List<Body> bodies;
    private SaveSystem saveSystem;
    private SaveData saveData;
    private SimulationData simData;
    private SimulationUI simUI;
    private TextureManager textureManager;
    private MouseState mouseState;
    private Vector2 mousePosition;
    private Test test;
    private GhostBody ghostBody;
    
    // PLACEHOLDER BODY DATA
    private float bodyDisplaySize = 0.05f;
    private Vector2 velocity = new(0.0f, 4.0f);
    
    public Simulation(Game game, string filePath)
    {
        simData = new SimulationData();
        simUI = new SimulationUI(game);
        bodies = new List<Body>();
        saveSystem = new SaveSystem();
        textureManager = new TextureManager();
        textureManager.LoadContent(game.Content, game.GraphicsDevice);
        mouseState = new MouseState();
        mousePosition = Vector2.Zero;
        test = new Test();
        ghostBody = new GhostBody(bodyDisplaySize);
        
        saveData = saveSystem.Load(filePath);

        if (saveData?.Bodies != null)
        {
            foreach (var bodyData in saveData.Bodies)
            {
                bodies.Add(new Body(bodyData.Name,
                    bodyData.Position,
                    bodyData.Velocity,
                    bodyData.Mass,
                    bodyData.DisplayRadius,
                    textureManager));
            }
        }
        
        Console.WriteLine($"DEBUG: Simulation loaded: {filePath}");
        
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(simUI.SettingsPanel(simData));
        rootContainer.Widgets.Add(simUI.ReturnButton());
        rootContainer.Widgets.Add(simUI.EditPanel(simData));
        
        desktop = new Desktop();
        desktop.Root = rootContainer;
        
        test.TestSimulationLoading(saveData.Bodies.Count, bodies.Count);
    }
    
    public override void Update(GameTime gameTime)
    {
        mouseState = Mouse.GetState();
        mousePosition = mouseState.Position.ToVector2();
        ghostBody.Update(mousePosition);

        if (simData.ToggleBodyGhost)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var body = new Body(
                    "Test Body",
                    mousePosition, 
                    velocity, 
                    2e6f, 
                    bodyDisplaySize, 
                    textureManager);
                
                bodies.Add(body);
                simData.ToggleBodyGhost = !simData.ToggleBodyGhost;
            }
        }
        
        if (!simData.IsPaused)
        {
            foreach (Body body in bodies)
            {
                body.Update(bodies, simData.TimeStep);
            }
        }
        
        simUI.PauseToggle(simData);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        foreach (Body body in bodies)
        {
            body.Draw(spriteBatch, simData);
        }

        if (simData.ToggleBodyGhost)
        {
            ghostBody.Draw(spriteBatch, textureManager);
        }
        
        spriteBatch.End();
        
        desktop.Render();
    }
}