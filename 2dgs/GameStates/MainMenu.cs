using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class MainMenu(Game game) : GameState
{
    private readonly MainMenuUi _mainMenuUi = new(game);
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;

    public override void Update(GameTime gameTime) {}

    private static Vector2 PositionAtCenter(float screenWidth, float screenHeight, Texture2D texture)
    {
        return new Vector2(screenWidth / 2.0f - texture.Width / 2.0f, screenHeight / 2.0f - texture.Height / 2.0f);
    }

    private static Vector2 PositionAtTop(float screenWidth, Texture2D texture, float padding = 0.0f, float scaleFactor = 1.0f)
    {
        screenWidth += padding;
        var correctedWidth = texture.Width * scaleFactor;
        return new Vector2(screenWidth / 2.0f - correctedWidth / 2.0f, 0.0f + padding);
    }

    private void DrawTitle(SpriteBatch spriteBatch)
    {
        var scaleFactor = ScreenHeight / 2160.0f;
        spriteBatch.Draw(_textureManager.AppTitle,
            PositionAtTop(ScreenWidth, _textureManager.AppTitle, scaleFactor: scaleFactor),
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
        spriteBatch.Draw(_textureManager.BackgroundTexture, PositionAtCenter(ScreenWidth, ScreenHeight, 
                _textureManager.BackgroundTexture), Color.White);
        spriteBatch.Draw(_textureManager.Gradient, PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), 
            Color.White);
        DrawTitle(spriteBatch);
        spriteBatch.End();
        _mainMenuUi.Draw();
    }
}