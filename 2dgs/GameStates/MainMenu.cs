using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class MainMenu : GameState
{
    private readonly MainMenuUi _mainMenuUi;

    public MainMenu(Game game)
    {
        _mainMenuUi = new MainMenuUi(game);
    }
    
    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _mainMenuUi.Draw();
    }
}