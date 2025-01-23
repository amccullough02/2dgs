using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class MainMenu(Game game) : GameState
{
    private readonly MainMenuUi _mainMenuUi = new(game);

    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _mainMenuUi.Draw();
    }
}