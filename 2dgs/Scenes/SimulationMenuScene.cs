using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A scene for providing a list of simulations available to the user.
/// </summary>
/// <param name="game">A reference to the MonoGame Game instance.</param>
public class SimulationMenuScene(Game game) : Scene
{
    /// <summary>
    /// The user interface of the Simulation Menu.
    /// </summary>
    private readonly SimulationMenuUi _simulationMenuUi = new(game);
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
    /// The update method of the Simulation Menu Scene.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    public override void Update(GameTime gameTime) {}

    /// <summary>
    /// The draw method for the Simulation Menu Scene.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.SimulationMenuBackground, TextureManager.PositionAtCenter(ScreenWidth, 
            ScreenHeight, _textureManager.SimulationMenuBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            TextureManager.PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), Color.White);
        spriteBatch.End();
        _simulationMenuUi.Draw();
    }
}