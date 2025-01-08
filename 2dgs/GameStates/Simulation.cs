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
    
    public Simulation(Game game, string filePath)
    {
        simData = new SimulationData();
        simUI = new SimulationUI(game);
        bodies = new List<Body>();
        saveSystem = new SaveSystem();
        
        saveData = saveSystem.Load(filePath);

        if (saveData?.Bodies != null)
        {
            foreach (var bodyData in saveData.Bodies)
            {
                bodies.Add(new Body(bodyData.Name, bodyData.Position, bodyData.Velocity, bodyData.Mass, bodyData.DisplayRadius));
            }
        }
        
        foreach (Body body in bodies)
        {
            body.LoadContent(game.Content, game.GraphicsDevice);
        }
        
        Console.WriteLine($"DEBUG: Simulation loaded: {filePath}");
        
        MyraEnvironment.Game = game;
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(simUI.SettingsPanel(simData));
        rootContainer.Widgets.Add(simUI.ReturnButton());
        
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
        
        spriteBatch.End();
        
        desktop.Render();
    }
}