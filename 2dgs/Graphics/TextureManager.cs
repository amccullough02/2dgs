using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class TextureManager
{
    public Texture2D BodyTexture { get; private set; }
    public Texture2D OrbitTexture { get; private set; }

    public TextureManager(ContentManager content, GraphicsDevice graphicsDevice)
    {
        BodyTexture = content.Load<Texture2D>("blank_circle");
        OrbitTexture = new Texture2D(graphicsDevice, 1, 1);
        OrbitTexture.SetData([Color.White]);
    }
}