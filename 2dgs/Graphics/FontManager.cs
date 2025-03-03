using System.IO;
using FontStashSharp;

namespace _2dgs;

/// <summary>
/// A class used to provide a facade for FontStashSharp FontSystems.
/// </summary>
public static class FontManager
{
    /// <summary>
    /// A font system for the League Spartan Light font face.
    /// </summary>
    private static readonly FontSystem LightFontSystem;
    /// <summary>
    /// A font system for the League Spartan Medium font face.
    /// </summary>
    private static readonly FontSystem MediumFontSystem;
    /// <summary>
    /// A font system for the League Spartan Bold font face.
    /// </summary>
    private static readonly FontSystem BoldFontSystem;
    /// <summary>
    /// A font system for the Space Grotesk Semi Bold font face.
    /// </summary>
    private static readonly FontSystem ButtonFontSystem;
    /// <summary>
    /// A font system for the Space Grotesk Medium font face, used for the titles of menu scenes.
    /// </summary>
    private static readonly FontSystem TitleFontSystem;

    /// <summary>
    /// The static constructor for the FontManager where fonts are added to the Font Systems from the fonts folder.
    /// </summary>
    static FontManager()
    {
        LightFontSystem = new FontSystem();
        LightFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/LeagueSpartan-Light.ttf"));
        MediumFontSystem = new FontSystem();
        MediumFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/LeagueSpartan-Medium.ttf"));
        BoldFontSystem = new FontSystem();
        BoldFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/LeagueSpartan-Bold.ttf"));
        ButtonFontSystem = new FontSystem();
        ButtonFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/SpaceGrotesk-Medium.ttf"));
        TitleFontSystem = new FontSystem();
        TitleFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/SpaceGrotesk-SemiBold.ttf"));
    }

    /// <summary>
    /// Helper method to obtain a light font.
    /// </summary>
    /// <param name="size">The font size.</param>
    /// <returns>A light font SpriteFontBase of the desired size.</returns>
    public static SpriteFontBase LightText(int size)
    {
        return LightFontSystem.GetFont(size);
    }

    /// <summary>
    /// Helper method to obtain a medium font.
    /// </summary>
    /// <param name="size">The font size.</param>
    /// <returns>A medium font SpriteFontBase of the desired size.</returns>
    public static SpriteFontBase MediumText(int size)
    {
        return MediumFontSystem.GetFont(size);
    }

    /// <summary>
    /// Helper method to obtain a bold font.
    /// </summary>
    /// <param name="size">The font size.</param>
    /// <returns>A bold font SpriteFontBase of the desired size.</returns>
    public static SpriteFontBase BoldText(int size)
    {
        return BoldFontSystem.GetFont(size);
    }

    /// <summary>
    /// Helper method to obtain a title font.
    /// </summary>
    /// <param name="size">The font size.</param>
    /// <returns>A title font SpriteFontBase of the desired size.</returns>
    public static SpriteFontBase TitleText(int size)
    {
        return TitleFontSystem.GetFont(size);
    }

    /// <summary>
    /// Helper method to obtain a button font.
    /// </summary>
    /// <param name="size">The font size.</param>
    /// <returns>A button font SpriteFontBase of the desired size.</returns>
    public static SpriteFontBase ButtonText(int size)
    {
        return ButtonFontSystem.GetFont(size);
    }
}