using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class SettingsMenu : GameState
{
    private SettingsMenuData _settingsMenuData;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private bool _remappingComplete = true;
    private readonly SettingsMenuUi _settingsMenuUi;

    public SettingsMenu(Game game)
    {
        _settingsMenuData = new SettingsMenuData();
        _settingsMenuUi = new SettingsMenuUi(game, _settingsMenuData);
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _settingsMenuUi.Draw();
    }
}