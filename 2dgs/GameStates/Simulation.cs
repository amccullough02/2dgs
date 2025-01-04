using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Simulation : GameState
{
    private Game game;
    private string filePath;
    private List<Body> bodies;
    private SaveSystem saveSystem;
    
    public Simulation(Game game, string filePath)
    {
        this.game = game;
        this.filePath = filePath;
        bodies = new List<Body>();
        saveSystem = new SaveSystem();
        Console.WriteLine($"DEBUG: Simulation loaded: {filePath}");
    }

    public override void Initialize()
    {
        SaveData saveData = saveSystem.Load(filePath);

        if (saveData?.Bodies != null)
        {

            foreach (var bodyData in saveData.Bodies)
            {
                bodies.Add(new Body(bodyData.Position, bodyData.Mass, bodyData.DisplayRadius));
            }

            Console.WriteLine("Simulation initialized");
        }
        else
        {
            Console.WriteLine("Cannot load bodies from save file");
        }
    }

    public override void LoadContent(ContentManager content)
    {
        foreach (Body body in bodies)
        {
            body.LoadContent(content);
        }
        Console.WriteLine("Simulation loaded");
    }
    public override void Update(GameTime gameTime)
    {
        foreach (Body body in bodies)
        {
            body.Update(bodies);
        }
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