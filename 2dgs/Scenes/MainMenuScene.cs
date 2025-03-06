using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A scene for the main menu of 2DGS.
/// </summary>
/// <param name="game">A reference to the MonoGame Game instance.</param>
public class MainMenuScene(Game game) : Scene
{
    /// <summary>
    /// The user interface of the Main Menu Scene.
    /// </summary>
    private readonly MainMenuUi _mainMenuUi = new(game);
    /// <summary>
    /// An instance of the TextureManager, used to provide this scene's background and gradient.
    /// </summary>
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    /// <summary>
    /// A reference to the current screen width.
    /// </summary>
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    /// <summary>
    /// A reference to the current screen height.
    /// </summary>
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;

    /// <summary>
    /// The update method of the Attributions Scene.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    public override void Update(GameTime gameTime) {}

    /// <summary>
    /// A helper method used to draw the title of the main menu.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    private void DrawTitle(SpriteBatch spriteBatch)
    {
        var scaleFactor = ScreenHeight / 2160.0f;
        spriteBatch.Draw(_textureManager.AppTitle,
            TextureManager.PositionAtTop(ScreenWidth, _textureManager.AppTitle, scaleFactor: scaleFactor),
            null,
            Color.White,
            0f,
            Vector2.Zero, 
            new Vector2(scaleFactor, scaleFactor),
            SpriteEffects.None,
            0f);
    }

    /// <summary>
    /// The draw method for the Main Menu Scene.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.MainMenuBackground, TextureManager.PositionAtCenter(ScreenWidth, ScreenHeight, 
                _textureManager.MainMenuBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            TextureManager.PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), Color.White);
        DrawTitle(spriteBatch);
        spriteBatch.End();
        _mainMenuUi.Draw();
    }
}