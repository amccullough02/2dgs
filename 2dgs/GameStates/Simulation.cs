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
        SaveSystem saveSystem = new SaveSystem();
        Console.WriteLine($"DEBUG: Simulation loaded: {filePath}");
    }

    public void Initialize()
    {
        SaveData saveData = saveSystem.Load(filePath);

        foreach (var bodyData in saveData.Bodies)
        {
            bodies.Add(new Body(bodyData.Position, bodyData.Mass, bodyData.DisplayRadius));
        }
    }

    public void LoadContent(ContentManager content)
    {
        foreach (Body body in bodies)
        {
            body.LoadContent(content);
        }
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