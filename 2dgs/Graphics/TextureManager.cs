using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class TextureManager
{
    public Texture2D BodyTexture { get; private set; }
    public Texture2D OrbitTexture { get; private set; }
    public Texture2D SelectorTexture { get; private set; }

    public void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        BodyTexture = content.Load<Texture2D>("blank_circle");
        SelectorTexture = content.Load<Texture2D>("selector");
        OrbitTexture = new Texture2D(graphics, 1, 1);
        OrbitTexture.SetData([Color.White]);
    }
}