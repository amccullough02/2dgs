using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsScene : Scene
{
    private readonly SettingsSceneData _settingsSceneData;
    private readonly TextureManager _textureManager;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private readonly float _screenWidth;
    private readonly float _screenHeight;
    private readonly SettingsMenuUi _settingsMenuUi;
    
    public SettingsScene(Game game)
    {
        _settingsSceneData = new SettingsSceneData();
        SetupDictionaries(_settingsSceneData);
        _screenHeight = game.GraphicsDevice.Viewport.Height;
        _screenWidth = game.GraphicsDevice.Viewport.Width;
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _settingsMenuUi = new SettingsMenuUi(game, _settingsSceneData);
    }
    
    private static void SetupDictionaries(SettingsSceneData settingsSceneData)
    {
        settingsSceneData.NewShortcuts.Add("PauseShortcut", []);
        settingsSceneData.NewShortcuts.Add("SpeedUpShortcut", []);
        settingsSceneData.NewShortcuts.Add("SpeedDownShortcut", []);
        settingsSceneData.NewShortcuts.Add("TrailsShortcut", []);
        settingsSceneData.NewShortcuts.Add("OrbitsShortcut", []);
        settingsSceneData.NewShortcuts.Add("VectorsShortcut", []);
        settingsSceneData.NewShortcuts.Add("NamesShortcut", []);
        settingsSceneData.NewShortcuts.Add("GlowShortcut", []);
        settingsSceneData.NewShortcuts.Add("EditShortcut", []);
        settingsSceneData.NewShortcuts.Add("ScreenshotShortcut", []);
        
        settingsSceneData.DefaultShortcuts.Add("PauseShortcut", [Keys.LeftControl, Keys.P]);
        settingsSceneData.DefaultShortcuts.Add("SpeedUpShortcut", [Keys.LeftControl, Keys.Right]);
        settingsSceneData.DefaultShortcuts.Add("SpeedDownShortcut", [Keys.LeftControl, Keys.Left]);
        settingsSceneData.DefaultShortcuts.Add("TrailsShortcut", [Keys.LeftControl, Keys.T]);
        settingsSceneData.DefaultShortcuts.Add("OrbitsShortcut", [Keys.LeftControl, Keys.O]);
        settingsSceneData.DefaultShortcuts.Add("VectorsShortcut", [Keys.LeftControl, Keys.V]);
        settingsSceneData.DefaultShortcuts.Add("NamesShortcut", [Keys.LeftControl, Keys.N]);
        settingsSceneData.DefaultShortcuts.Add("GlowShortcut", [Keys.LeftControl, Keys.G]);
        settingsSceneData.DefaultShortcuts.Add("EditShortcut", [Keys.LeftControl, Keys.E]);
        settingsSceneData.DefaultShortcuts.Add("ScreenshotShortcut", [Keys.F11]);
    }

    public override void Update(GameTime gameTime)
    {
        if (_settingsSceneData.Remapping)
        {
            _keyboardState = Keyboard.GetState();

            foreach (var key in _keyboardState.GetPressedKeys())
            {
                if (_previousKeyboardState.IsKeyDown(key)) continue;
                
                if (_settingsSceneData.NewShortcuts.ContainsKey(_settingsSceneData.WhichShortcut))
                {
                    _settingsSceneData.NewShortcuts[_settingsSceneData.WhichShortcut].Add(key);
                }
            }
            
            _previousKeyboardState = _keyboardState;

            _settingsSceneData.ShortcutPreview  = StringTransformer.KeybindString(_settingsSceneData.NewShortcuts[_settingsSceneData.WhichShortcut]);
        }

        if (_settingsSceneData.ClearShortcut)
        {
            _settingsSceneData.NewShortcuts[_settingsSceneData.WhichShortcut].Clear();
            _settingsSceneData.ClearShortcut = false;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.SettingsBackground, _textureManager.PositionAtCenter(_screenWidth, _screenHeight, 
            _textureManager.SettingsBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            _textureManager.PositionAtCenter(_screenWidth, _screenHeight, _textureManager.Gradient), Color.White);
        spriteBatch.End();
        _settingsMenuUi.Draw();
    }
}