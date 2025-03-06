using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A scene for giving credit and attribution.
/// </summary>
/// <param name="game">A reference to the MonoGame Game instance.</param>
public class AttributionsScene(Game game) : Scene
{
    /// <summary>
    /// The user interface of the Attributions Scene.
    /// </summary>
    private readonly AttributionsUi _attributionsUi = new(game);
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
    /// The draw method for the Attributions Scene.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.AttributionsBackground, TextureManager.PositionAtCenter(ScreenWidth, ScreenHeight, 
            _textureManager.MainMenuBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            TextureManager.PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), Color.White);
        spriteBatch.End();
        _attributionsUi.Draw();
    }
}