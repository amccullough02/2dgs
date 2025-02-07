using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class MainMenuScene(Game game) : Scene
{
    private readonly MainMenuUi _mainMenuUi = new(game);
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;

    public override void Update(GameTime gameTime) {}

    private void DrawTitle(SpriteBatch spriteBatch)
    {
        var scaleFactor = ScreenHeight / 2160.0f;
        spriteBatch.Draw(_textureManager.AppTitle,
            _textureManager.PositionAtTop(ScreenWidth, _textureManager.AppTitle, scaleFactor: scaleFactor),
            null,
            Color.White,
            0f,
            Vector2.Zero, 
            new Vector2(scaleFactor, scaleFactor),
            SpriteEffects.None,
            0f);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.MainMenuBackground, _textureManager.PositionAtCenter(ScreenWidth, ScreenHeight, 
                _textureManager.MainMenuBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            _textureManager.PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), Color.White);
        DrawTitle(spriteBatch);
        spriteBatch.End();
        _mainMenuUi.Draw();
    }
}