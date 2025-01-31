using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class TextureManager
{
    
    public Texture2D BackgroundTexture { get; private set; }
    public Texture2D Gradient { get; private set; }
    public Texture2D AppTitle  { get; private set; }
    public Texture2D BodyTexture { get; private set; }
    public Texture2D OrbitTexture { get; }

    public TextureManager(ContentManager content, GraphicsDevice graphicsDevice)
    {
        BackgroundTexture = content.Load<Texture2D>("images/main_menu_background");
        Gradient = content.Load<Texture2D>("images/gradient");
        AppTitle = content.Load<Texture2D>("images/title");
        BodyTexture = content.Load<Texture2D>("images/blank_circle");
        OrbitTexture = new Texture2D(graphicsDevice, 1, 1);
        OrbitTexture.SetData([Color.White]);
    }
}