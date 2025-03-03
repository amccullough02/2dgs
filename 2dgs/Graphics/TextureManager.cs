using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A data class used for handling the textures used in the 2DGS application.
/// </summary>
public class TextureManager
{
    /// <summary>
    /// The main menu's background texture.
    /// </summary>
    public Texture2D MainMenuBackground { get; private set; }
    /// <summary>
    /// The Attribution screen's background texture.
    /// </summary>
    public Texture2D AttributionsBackground { get; private set; }
    /// <summary>
    /// The settings menu's background texture.
    /// </summary>
    public Texture2D SettingsBackground { get; private set; }
    /// <summary>
    /// The simulation menu's background texture.
    /// </summary>
    public Texture2D SimulationMenuBackground { get; private set; }
    /// <summary>
    /// The background using in simulation scenes.
    /// </summary>
    public Texture2D SimulationBackground { get; private set; }
    /// <summary>
    /// The default gradient applied to all background textures.
    /// </summary>
    public Texture2D Gradient { get; private set; }
    /// <summary>
    /// The application title texture.
    /// </summary>
    public Texture2D AppTitle  { get; private set; }
    /// <summary>
    /// The base texture used by bodies.
    /// </summary>
    public Texture2D BodyTexture { get; private set; }
    /// <summary>
    /// The texture used to draw the tip of a velocity vector arrow.
    /// </summary>
    public Texture2D ArrowTip { get; private set; }
    /// <summary>
    /// A procedural texture used to draw rectangles.
    /// </summary>
    public Texture2D BaseTexture { get; }

    public TextureManager(ContentManager content, GraphicsDevice graphicsDevice)
    {
        MainMenuBackground = content.Load<Texture2D>("images/main_menu_background");
        AttributionsBackground = content.Load<Texture2D>("images/attributions_background");
        SettingsBackground = content.Load<Texture2D>("images/settings_background");
        SimulationMenuBackground = content.Load<Texture2D>("images/sim_menu_background");
        SimulationBackground = content.Load<Texture2D>("images/simulation_background");
        Gradient = content.Load<Texture2D>("images/gradient");
        AppTitle = content.Load<Texture2D>("images/title");
        BodyTexture = content.Load<Texture2D>("images/blank_circle");
        ArrowTip = content.Load<Texture2D>("images/arrow_tip");
        BaseTexture = new Texture2D(graphicsDevice, 1, 1);
        BaseTexture.SetData([Color.White]);
    }
    
    /// <summary>
    /// A static method used to position a texture in the centre of the screen, typically used for background textures.
    /// </summary>
    /// <param name="screenWidth">The current width of the screen.</param>
    /// <param name="screenHeight">The current height of the screen.</param>
    /// <param name="texture">The texture to position.</param>
    /// <returns></returns>
    public static Vector2 PositionAtCenter(float screenWidth, float screenHeight, Texture2D texture)
    {
        return new Vector2(screenWidth / 2.0f - texture.Width / 2.0f, screenHeight / 2.0f - texture.Height / 2.0f);
    }
    
    /// <summary>
    /// A static method used to position a texture at the top-centre of the screen.
    /// </summary>
    /// <param name="screenWidth">The current width of the screen.</param>
    /// <param name="texture">The texture to position.</param>
    /// <param name="padding">The amount of padding to add between the top of the screen and the top of the texture rectangle.</param>
    /// <param name="scaleFactor">The scale of the texture (used if the texture is to change size based on vertical resolution).</param>
    /// <returns></returns>
    public static Vector2 PositionAtTop(float screenWidth, Texture2D texture, float padding = 0.0f, float scaleFactor = 1.0f)
    {
        screenWidth += padding;
        var correctedWidth = texture.Width * scaleFactor;
        return new Vector2(screenWidth / 2.0f - correctedWidth / 2.0f, 0.0f + padding);
    }
}