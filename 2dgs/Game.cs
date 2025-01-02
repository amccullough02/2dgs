using System;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    public GraphicsDeviceManager _graphics { get; }
    private SpriteBatch _spriteBatch;
    private Test _test;
    public FpsCounter _fpsCounter { get; private set; }
    
    public GameStateManager GameStateManager { get; private set; }

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = false;
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges();
        _test = new Test();
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        _test.RunAllTests(_graphics, Window.Title);
        
        GameStateManager = new GameStateManager();
        GameStateManager.PushState(new MainMenu(this));
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _fpsCounter = new FpsCounter();
    }

    protected override void Update(GameTime gameTime)
    {
        _fpsCounter.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        GameStateManager.Draw(gameTime, _spriteBatch);
        
        _fpsCounter.Draw(_spriteBatch);
        
        base.Draw(gameTime);
    }
}