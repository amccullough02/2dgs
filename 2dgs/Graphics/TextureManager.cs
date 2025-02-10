using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class TextureManager
{
    public Texture2D MainMenuBackground { get; private set; }
    public Texture2D AttributionsBackground { get; private set; }
    public Texture2D SettingsBackground { get; private set; }
    public Texture2D SimulationMenuBackground { get; private set; }
    public Texture2D Gradient { get; private set; }
    public Texture2D AppTitle  { get; private set; }
    public Texture2D BodyTexture { get; private set; }
    public Texture2D ArrowTip { get; private set; }
    public Texture2D BaseTexture { get; }

    public TextureManager(ContentManager content, GraphicsDevice graphicsDevice)
    {
        MainMenuBackground = content.Load<Texture2D>("images/main_menu_background");
        AttributionsBackground = content.Load<Texture2D>("images/attributions_background");
        SettingsBackground = content.Load<Texture2D>("images/settings_background");
        SimulationMenuBackground = content.Load<Texture2D>("images/sim_menu_background");
        Gradient = content.Load<Texture2D>("images/gradient");
        AppTitle = content.Load<Texture2D>("images/title");
        BodyTexture = content.Load<Texture2D>("images/blank_circle");
        ArrowTip = content.Load<Texture2D>("images/arrow_tip");
        BaseTexture = new Texture2D(graphicsDevice, 1, 1);
        BaseTexture.SetData([Color.White]);
    }
    
    public Vector2 PositionAtCenter(float screenWidth, float screenHeight, Texture2D texture)
    {
        return new Vector2(screenWidth / 2.0f - texture.Width / 2.0f, screenHeight / 2.0f - texture.Height / 2.0f);
    }
    
    public Vector2 PositionAtTop(float screenWidth, Texture2D texture, float padding = 0.0f, float scaleFactor = 1.0f)
    {
        screenWidth += padding;
        var correctedWidth = texture.Width * scaleFactor;
        return new Vector2(screenWidth / 2.0f - correctedWidth / 2.0f, 0.0f + padding);
    }
}