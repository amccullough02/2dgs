using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// The 2DGS game class, inherits from the MonoGame Game class.
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{
    /// <summary>
    /// An instance of the MonoGame GraphicsDeviceManager class.
    /// </summary>
    public GraphicsDeviceManager Graphics { get; }
    /// <summary>
    /// An instance of the 2DGS FpsCounter class.
    /// </summary>
    public FpsCounter FpsCounter { get; private set; }
    /// <summary>
    /// An instance of the 2DGS SceneManager class.
    /// </summary>
    public SceneManager SceneManager { get; private set; }
    /// <summary>
    /// An instance of the 2DGS SaveSystem class.
    /// </summary>
    public readonly SaveSystem SaveSystem;
    private MusicPlayer _musicPlayer;
    private SpriteBatch _spriteBatch;

    /// <summary>
    /// The constructor for the 2DGS game class.
    /// </summary>
    public Game()
    {
        IsMouseVisible = true;
        IsFixedTimeStep = false;
        Content.RootDirectory = "Content";
        SaveSystem = new SaveSystem();
        var settingsSaveData = SaveSystem.LoadSettings();
        Graphics = new GraphicsDeviceManager(this);
        SetupGraphics(Graphics, settingsSaveData);
        SetupGlobalComponents();
    }

    /// <summary>
    /// A helper method to encapsulate graphics initialisation.
    /// </summary>
    /// <param name="graphics">A reference to the MonoGame GraphicsDeviceManager class.</param>
    /// <param name="settingsSaveData">A reference to the SettingsSaveData class.</param>
    private void SetupGraphics(GraphicsDeviceManager graphics, SettingsSaveData settingsSaveData)
    {
        graphics.PreferredBackBufferHeight = settingsSaveData.VerticalResolution;
        graphics.PreferredBackBufferWidth = settingsSaveData.HorizontalResolution;
        graphics.IsFullScreen = settingsSaveData.Fullscreen;
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.GraphicsProfile = GraphicsProfile.HiDef;
        graphics.ApplyChanges();
    }

    /// <summary>
    /// A helper method to initialise 'global' components, such as the music player.
    /// </summary>
    private void SetupGlobalComponents()
    {
        _musicPlayer = new MusicPlayer(Content);
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        SceneManager = new SceneManager();
        SceneManager.PushScene(new MainMenuScene(this));
        // SceneManager.PushScene(new SimulationScene(this, "../../../savedata/lessons/galilean_system.json"));
        _musicPlayer.Initialize();
        RunTests();
        base.Initialize();
    }

    /// <summary>
    /// Runs the high-level 2DGS tests associated with application setup and initialisation.
    /// </summary>
    private void RunTests()
    {
        TestRunner.AssertApplicationName(Window.Title, "2DGS - Alpha");
        TestRunner.AssertApplicationResolution(Graphics, SaveSystem.LoadSettings());
        TestRunner.AssertApplicationDisplayMode(Graphics, SaveSystem.LoadSettings());
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        FpsCounter = new FpsCounter();
    }

    protected override void Update(GameTime gameTime)
    {
        FpsCounter.Update(gameTime);
        SceneManager.Update(gameTime);
        TestRunner.SaveResults();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SceneManager.Draw(gameTime, _spriteBatch);
        FpsCounter.Draw(_spriteBatch);
        base.Draw(gameTime);
    }

    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        _musicPlayer.Dispose();
        base.OnExiting(sender, args);
    }
}