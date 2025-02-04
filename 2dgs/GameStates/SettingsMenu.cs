using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsMenu : GameState
{
    private SettingsMenuData _settingsMenuData;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private readonly SettingsMenuUi _settingsMenuUi;

    public SettingsMenu(Game game)
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
        _settingsMenuUi.Draw();
    }
}