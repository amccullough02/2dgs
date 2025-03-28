using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

/// <summary>
/// A class used to create a ghost body when positioning a newly created body.
/// </summary>
public class GhostBody
{
    /// <summary>
    /// The position of the ghost body.
    /// </summary>
    public Vector2 Position { get; set; } = Vector2.Zero;
    /// <summary>
    /// The display size of the ghost body.
    /// </summary>
    private float DisplaySize { get; set; } = 0.1f;

    /// <summary>
    /// The update method for the GhostBody, the position will update with the position of the mouse cursor.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    public void Update(SimulationMediator simulationMediator)
    {
        var mouseState = Mouse.GetState();
        Position = mouseState.Position.ToVector2();

        if (simulationMediator.ToggleBodyGhost)
        {
            DisplaySize = simulationMediator.CreateBodyData.Diameter;
        }
    }

    /// <summary>
    /// The draw method for the GhostBody, it is drawn at a lower opacity to signify that the body is not yet a part of the simulation.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="textureManager">A reference to the TextureManager class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, SimulationMediator simulationMediator)
    {
        if (!simulationMediator.ToggleBodyGhost) return;
        if (simulationMediator.ToggleGlow)
        {
            for (var i = 0; i < 100; i++)
            {
                var glowOpacity = 0.07f - (i * 0.002f);
                var glowRadius = 1.0f + (i * 0.02f);
            
                spriteBatch.Draw(textureManager.BodyTexture,
                    Position,
                    null,
                    Color.White * glowOpacity,
                    0f,
                    new Vector2(textureManager.BodyTexture.Width / 2.0f, textureManager.BodyTexture.Height / 2.0f),
                    new Vector2(DisplaySize * glowRadius, DisplaySize * glowRadius),
                    SpriteEffects.None,
                    0f);
            } 
        }
            
        spriteBatch.Draw(textureManager.BodyTexture,
            Position,
            null,
            Color.White * 0.5f,
            0f,
            new Vector2(textureManager.BodyTexture.Width / 2.0f, textureManager.BodyTexture.Height / 2.0f),
            new Vector2(DisplaySize, DisplaySize),
            SpriteEffects.None,
            0f);
    }
}