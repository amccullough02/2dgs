using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    public GraphicsDeviceManager _graphics { get; }
    private SpriteBatch _spriteBatch;
    private Test _test;
    
    public GameStateManager GameStateManager { get; private set; }

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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
        
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        GameStateManager.Draw(gameTime, _spriteBatch);
        base.Draw(gameTime);
    }
}