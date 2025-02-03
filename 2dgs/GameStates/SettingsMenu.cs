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
        _settingsMenuUi = new SettingsMenuUi(game, _settingsMenuData);
    }

    public override void Update(GameTime gameTime)
    {
        if (_settingsMenuData.Remapping)
        {
            _keyboardState = Keyboard.GetState();

            foreach (var key in _keyboardState.GetPressedKeys())
            {
                if (!_previousKeyboardState.IsKeyDown(key))
                {
                    _settingsMenuData.NewShortcut.Add(key);
                }
            }
            
            _previousKeyboardState = _keyboardState;

            _settingsMenuData.ShortcutPreview  = StringTransformer.KeybindString(_settingsMenuData.NewShortcut);
        }

        if (_settingsMenuData.ClearShortcut)
        {
            _settingsMenuData.NewShortcut.Clear();
            _settingsMenuData.ClearShortcut = false;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _settingsMenuUi.Draw();
    }
}