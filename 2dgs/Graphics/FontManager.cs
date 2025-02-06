using System.IO;
using FontStashSharp;

namespace _2dgs;

public static class FontManager
{
    private static readonly FontSystem LightFontSystem;
    private static readonly FontSystem MediumFontSystem;
    private static readonly FontSystem BoldFontSystem;
    private static readonly FontSystem TitleFontSystem;

    static FontManager()
    {
        LightFontSystem = new FontSystem();
        LightFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/LeagueSpartan-Light.ttf"));
        MediumFontSystem = new FontSystem();
        MediumFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/LeagueSpartan-Medium.ttf"));
        BoldFontSystem = new FontSystem();
        BoldFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/LeagueSpartan-Bold.ttf"));
        TitleFontSystem = new FontSystem();
        TitleFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/SpaceGrotesk-SemiBold.ttf"));
    }

    public static SpriteFontBase LightFont(int size)
    {
        return LightFontSystem.GetFont(size);
    }

    public static SpriteFontBase MediumFont(int size)
    {
        return MediumFontSystem.GetFont(size);
    }

    public static SpriteFontBase BoldFont(int size)
    {
        return BoldFontSystem.GetFont(size);
    }

    public static SpriteFontBase TitleFont(int size)
    {
        return TitleFontSystem.GetFont(size);
    }
}