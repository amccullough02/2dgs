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
    
    // TEMP
    private float bodyDisplaySize = 0.1f;
    private MouseState mouseState;
    private Vector2 mousePosition;
    
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
        
        saveData = saveSystem.Load(filePath);

        if (saveData?.Bodies != null)
        {
            foreach (var bodyData in saveData.Bodies)
            {
                bodies.Add(new Body(bodyData.Name, bodyData.Position, bodyData.Velocity, bodyData.Mass, bodyData.DisplayRadius, textureManager));
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
        
        TestSimulationLoading();
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
        mouseState = Mouse.GetState();
        mousePosition = mouseState.Position.ToVector2();

        if (simData.ToggleBodyGhost)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // CREATE BODY LOGIC
                var body = new Body("Test Body", mousePosition, Vector2.Zero, 2e6f, bodyDisplaySize, textureManager);
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
            spriteBatch.Draw(textureManager.BodyTexture,
                mousePosition,
                null,
                Color.White * 0.5f,
                0f,
                new Vector2(textureManager.BodyTexture.Width / 2, textureManager.BodyTexture.Height / 2),
                new Vector2(bodyDisplaySize, bodyDisplaySize),
                SpriteEffects.None,
                0f);
        }
        
        spriteBatch.End();
        
        desktop.Render();
    }
}