using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class GhostBody
{
    public Vector2 Position { get; set; }
    public float displaySize { get; set; }

    public GhostBody(float displaySize)
    {
        this.displaySize = displaySize;
        Position = new Vector2(0, 0);
    }

    public void Update(Vector2 position)
    {
        Position = position;
    }

    public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
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