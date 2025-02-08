using System.IO;
using FontStashSharp;

namespace _2dgs;

public static class FontManager
{
    private static readonly FontSystem LightFontSystem;
    private static readonly FontSystem MediumFontSystem;
    private static readonly FontSystem BoldFontSystem;
    public static readonly FontSystem ButtonFontSystem;
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
        ButtonFontSystem = new FontSystem();
        ButtonFontSystem.AddFont(File.ReadAllBytes("../../../Content/fonts/SpaceGrotesk-Medium.ttf"));
    }

    public static SpriteFontBase LightText(int size)
    {
        return LightFontSystem.GetFont(size);
    }

    public static SpriteFontBase MediumText(int size)
    {
        return MediumFontSystem.GetFont(size);
    }

    public static SpriteFontBase BoldText(int size)
    {
        return BoldFontSystem.GetFont(size);
    }

    public static SpriteFontBase TitleText(int size)
    {
        return TitleFontSystem.GetFont(size);
    }

    public static SpriteFontBase ButtonText(int size)
    {
        return ButtonFontSystem.GetFont(size);
    }
}