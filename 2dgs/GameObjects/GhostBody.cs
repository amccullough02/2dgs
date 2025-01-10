using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class GhostBody
{
    public Vector2 Position { get; set; }
    public float displaySize { get; set; }

    public GhostBody()
    {
        displaySize = 0.1f;
        Position = Vector2.Zero;
    }
    
    public GhostBody(float displaySize)
    {
        this.displaySize = displaySize;
        Position = Vector2.Zero;
    }

    public void Update(SimulationData simulationData)
    {
        var mouseState = Mouse.GetState();
        Position = mouseState.Position.ToVector2();

        if (simulationData.ToggleBodyGhost)
        {
            displaySize = simulationData.CreateBodyData.DisplayRadius;
        }
    }

    public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, SimulationData simulationData)
    {
        if (simulationData.ToggleBodyGhost)
        {
            spriteBatch.Draw(textureManager.BodyTexture,
                Position,
                null,
                Color.White * 0.5f,
                0f,
                new Vector2(textureManager.BodyTexture.Width / 2, textureManager.BodyTexture.Height / 2),
                new Vector2(displaySize, displaySize),
                SpriteEffects.None,
                0f);
        }
    }
}