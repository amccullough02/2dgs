﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    
    private SpriteBatch _spriteBatch;
    private readonly Test _test;
    public GraphicsDeviceManager Graphics { get; }
    public FpsCounter FpsCounter { get; private set; }
    public GameStateManager GameStateManager { get; private set; }

    public Game()
    {
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferHeight = 1080;
        Graphics.PreferredBackBufferWidth = 1920;
        Graphics.SynchronizeWithVerticalRetrace = true;
        Graphics.GraphicsProfile = GraphicsProfile.HiDef;
        Graphics.ApplyChanges();
        IsMouseVisible = true;
        IsFixedTimeStep = false;
        Content.RootDirectory = "Content";
        _test = new Test();
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        _test.RunAllTests(Graphics, Window.Title);
        GameStateManager = new GameStateManager();
        GameStateManager.PushState(new Simulation(this, "../../../sims/lessons/tutorial.json"));
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        FpsCounter = new FpsCounter();
    }

    protected override void Update(GameTime gameTime)
    {
        FpsCounter.Update(gameTime);
        GameStateManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        FpsCounter.Draw(_spriteBatch);
        GameStateManager.Draw(gameTime, _spriteBatch);
        base.Draw(gameTime);
    }
}