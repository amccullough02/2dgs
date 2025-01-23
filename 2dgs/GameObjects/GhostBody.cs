﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class GhostBody
{
    public Vector2 Position { get; set; }
    private float DisplaySize { get; set; }

    public GhostBody()
    {
        DisplaySize = 0.1f;
        Position = Vector2.Zero;
    }

    public void Update(SimulationData simulationData)
    {
        var mouseState = Mouse.GetState();
        Position = mouseState.Position.ToVector2();

        if (simulationData.ToggleBodyGhost)
        {
            DisplaySize = simulationData.CreateBodyData.DisplayRadius;
        }
    }

    public void Draw(SpriteBatch spriteBatch, TextureManager textureManager, SimulationData simulationData)
    {
        if (simulationData.ToggleBodyGhost)
        {
            if (simulationData.ToggleGlow)
            {
                for (int i = 0; i < 100; i++)
                {
                    float glowOpacity = 0.07f - (i * 0.002f);
                    float glowRadius = 1.0f + (i * 0.02f);
            
                    spriteBatch.Draw(textureManager.BodyTexture,
                        Position,
                        null,
                        Color.White * glowOpacity,
                        0f,
                        new Vector2(textureManager.BodyTexture.Width / 2, textureManager.BodyTexture.Height / 2),
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
                new Vector2(textureManager.BodyTexture.Width / 2, textureManager.BodyTexture.Height / 2),
                new Vector2(DisplaySize, DisplaySize),
                SpriteEffects.None,
                0f);
        }
    }
}