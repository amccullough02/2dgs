using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class SettingsMenu : GameState
{
    private SettingsMenuUi _settingsMenuUi;
    
    public SettingsMenu(Game game)
    {
        _settingsMenuUi = new SettingsMenuUi(game);
    }
    
    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _settingsMenuUi.Draw();
    }
}