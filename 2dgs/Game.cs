using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    public GraphicsDeviceManager Graphics { get; }
    public FpsCounter FpsCounter { get; private set; }
    public GameStateManager GameStateManager { get; private set; }
    public readonly SaveSystem SaveSystem;
    private SpriteBatch _spriteBatch;
    private readonly Test _test;
    private readonly AudioPlayer _audioPlayer;

    public Game()
    {
        SaveSystem = new SaveSystem();
        var settingsSaveData = SaveSystem.LoadSettings();
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferHeight = settingsSaveData.VerticalResolution;
        Graphics.PreferredBackBufferWidth = settingsSaveData.HorizontalResolution;
        Graphics.IsFullScreen = settingsSaveData.Fullscreen;
        Graphics.SynchronizeWithVerticalRetrace = true;
        Graphics.GraphicsProfile = GraphicsProfile.HiDef;
        Graphics.ApplyChanges();
        IsMouseVisible = true;
        IsFixedTimeStep = false;
        Content.RootDirectory = "Content";
        _audioPlayer = new AudioPlayer(Content);
        _test = new Test();
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        _test.RunAllTests(Graphics, Window.Title);
        GameStateManager = new GameStateManager();
        GameStateManager.PushState(new MainMenu(this));
        // GameStateManager.PushState(new Simulation(this, "../../../savedata/lessons/tutorial.json"));
        _audioPlayer.PlayBgm();
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

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        _audioPlayer.Dispose();
        base.OnExiting(sender, args);
    }
}