using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class GhostBody
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    private float DisplaySize { get; set; } = 0.1f;

    public void Update(SimulationMediator simulationMediator)
    {
        var mouseState = Mouse.GetState();
        Position = mouseState.Position.ToVector2();

        if (simulationMediator.ToggleBodyGhost)
        {
            DisplaySize = simulationMediator.CreateBodyData.DisplaySize;
        }
    }

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