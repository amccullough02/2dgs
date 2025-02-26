using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// An abstract class that provides a standardized implementation for 2DGS scenes.
/// </summary>
public abstract class Scene
{
    /// <summary>
    /// An abstract method for updating the contents of a scene.
    /// </summary>
    /// <param name="gameTime">Access for MonoGame's GameTime class.</param>
    public abstract void Update(GameTime gameTime);
    /// <summary>
    /// An abstract method for drawing the contents of a scene.
    /// </summary>
    /// <param name="gameTime">Access for MonoGame's GameTime class.</param>
    /// <param name="spriteBatch">Access for MonoGame's SpriteBatch class.</param>
    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}