using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class SettingsMenu(Game game) : GameState
{
    private readonly SettingsMenuUi _settingsMenuUi = new(game);

    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _settingsMenuUi.Draw();
    }
}