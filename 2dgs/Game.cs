﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Game : Microsoft.Xna.Framework.Game
{
    public GraphicsDeviceManager Graphics { get; }
    public FpsCounter FpsCounter { get; private set; }
    public SceneManager SceneManager { get; private set; }
    public readonly SaveSystem SaveSystem;
    private Test _test;
    private MusicPlayer _musicPlayer;
    private SpriteBatch _spriteBatch;

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

    private void SetupGraphics(GraphicsDeviceManager graphics, SettingsSaveData settingsSaveData)
    {
        graphics.PreferredBackBufferHeight = settingsSaveData.VerticalResolution;
        graphics.PreferredBackBufferWidth = settingsSaveData.HorizontalResolution;
        graphics.IsFullScreen = settingsSaveData.Fullscreen;
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.GraphicsProfile = GraphicsProfile.HiDef;
        graphics.ApplyChanges();
    }

    private void SetupGlobalComponents()
    {
        _musicPlayer = new MusicPlayer(Content);
        _test = new Test();
    }

    protected override void Initialize()
    {
        Window.Title = "2DGS - Alpha";
        _test.RunAllTests(Graphics, Window.Title);
        SceneManager = new SceneManager();
        SceneManager.PushScene(new SimulationScene(this, "../../../savedata/lessons/galilean_system.json"));
        _musicPlayer.Initialize();
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
        SceneManager.Update(gameTime);
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