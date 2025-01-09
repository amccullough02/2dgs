using System.IO;
using FontStashSharp;

namespace _2dgs;

public class FontManager
{
    private FontSystem _lightFontSystem;
    private FontSystem _mediumFontSystem;
    private FontSystem _boldFontSystem;

    public FontManager()
    {
        _lightFontSystem = new FontSystem();
        _lightFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/LeagueSpartan-Light.ttf"));
        _mediumFontSystem = new FontSystem();
        _mediumFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/LeagueSpartan-Medium.ttf"));
        _boldFontSystem = new FontSystem();
        _boldFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/LeagueSpartan-Bold.ttf"));
    }

    public SpriteFontBase LightFont(int size)
    {
        return _lightFontSystem.GetFont(size);
    }

    public SpriteFontBase MediumFont(int size)
    {
        return _mediumFontSystem.GetFont(size);
    }

    public SpriteFontBase BoldFont(int size)
    {
        return _boldFontSystem.GetFont(size);
    }
}