using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingScene : Scene
{
    private readonly SettingsMenuData _settingsMenuData;
    private readonly TextureManager _textureManager;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private readonly float _screenWidth;
    private readonly float _screenHeight;
    private readonly SettingsMenuUi _settingsMenuUi;
    

    public SettingScene(Game game)
    {
        _settingsMenuData = new SettingsMenuData();
        _settingsMenuData.NewShortcuts.Add("PauseShortcut", []);
        _settingsMenuData.NewShortcuts.Add("SpeedUpShortcut", []);
        _settingsMenuData.NewShortcuts.Add("SpeedDownShortcut", []);
        _settingsMenuData.NewShortcuts.Add("TrailsShortcut", []);
        _settingsMenuData.NewShortcuts.Add("NamesShortcut", []);
        _settingsMenuData.NewShortcuts.Add("GlowShortcut", []);
        _settingsMenuData.NewShortcuts.Add("EditShortcut", []);
        _settingsMenuData.NewShortcuts.Add("ScreenshotShortcut", []);
        
        _screenHeight = game.GraphicsDevice.Viewport.Height;
        _screenWidth = game.GraphicsDevice.Viewport.Width;
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _settingsMenuUi = new SettingsMenuUi(game, _settingsMenuData);
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