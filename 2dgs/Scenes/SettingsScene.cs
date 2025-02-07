using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsScene : Scene
{
    private readonly SettingsMenuData _settingsMenuData;
    private readonly TextureManager _textureManager;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private readonly float _screenWidth;
    private readonly float _screenHeight;
    private readonly SettingsMenuUi _settingsMenuUi;
    
    public SettingsScene(Game game)
    {
        _settingsMenuData = new SettingsMenuData();
        SetupDictionaries(_settingsMenuData);
        _screenHeight = game.GraphicsDevice.Viewport.Height;
        _screenWidth = game.GraphicsDevice.Viewport.Width;
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _settingsMenuUi = new SettingsMenuUi(game, _settingsMenuData);
    }
    
    private static void SetupDictionaries(SettingsMenuData settingsMenuData)
    {
        settingsMenuData.NewShortcuts.Add("PauseShortcut", []);
        settingsMenuData.NewShortcuts.Add("SpeedUpShortcut", []);
        settingsMenuData.NewShortcuts.Add("SpeedDownShortcut", []);
        settingsMenuData.NewShortcuts.Add("TrailsShortcut", []);
        settingsMenuData.NewShortcuts.Add("NamesShortcut", []);
        settingsMenuData.NewShortcuts.Add("GlowShortcut", []);
        settingsMenuData.NewShortcuts.Add("EditShortcut", []);
        settingsMenuData.NewShortcuts.Add("ScreenshotShortcut", []);
        
        settingsMenuData.DefaultShortcuts.Add("PauseShortcut", [Keys.LeftControl, Keys.P]);
        settingsMenuData.DefaultShortcuts.Add("SpeedUpShortcut", [Keys.LeftControl, Keys.Right]);
        settingsMenuData.DefaultShortcuts.Add("SpeedDownShortcut", [Keys.LeftControl, Keys.Left]);
        settingsMenuData.DefaultShortcuts.Add("TrailsShortcut", [Keys.LeftControl, Keys.T]);
        settingsMenuData.DefaultShortcuts.Add("NamesShortcut", [Keys.LeftControl, Keys.N]);
        settingsMenuData.DefaultShortcuts.Add("GlowShortcut", [Keys.LeftControl, Keys.G]);
        settingsMenuData.DefaultShortcuts.Add("EditShortcut", [Keys.LeftControl, Keys.E]);
        settingsMenuData.DefaultShortcuts.Add("ScreenshotShortcut", [Keys.F11]);
    }

    public override void Update(GameTime gameTime)
    {
        if (_settingsMenuData.Remapping)
        {
            _keyboardState = Keyboard.GetState();

            foreach (var key in _keyboardState.GetPressedKeys())
            {
                if (_previousKeyboardState.IsKeyDown(key)) continue;
                
                if (_settingsMenuData.NewShortcuts.ContainsKey(_settingsMenuData.WhichShortcut))
                {
                    _settingsMenuData.NewShortcuts[_settingsMenuData.WhichShortcut].Add(key);
                }
            }
            
            _previousKeyboardState = _keyboardState;

            _settingsMenuData.ShortcutPreview  = StringTransformer.KeybindString(_settingsMenuData.NewShortcuts[_settingsMenuData.WhichShortcut]);
        }

        if (_settingsMenuData.ClearShortcut)
        {
            _settingsMenuData.NewShortcuts[_settingsMenuData.WhichShortcut].Clear();
            _settingsMenuData.ClearShortcut = false;
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