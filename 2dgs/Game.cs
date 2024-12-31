using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameStateManager _gameStateManager;
    private Test test;

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        test = new Test();
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        test.RunAllTests(_graphics, Window.Title);
        
        _gameStateManager = new GameStateManager();
        _gameStateManager.PushState(new MainMenu(this));
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
        _gameStateManager.Draw(gameTime, _spriteBatch);
        base.Draw(gameTime);
    }
}