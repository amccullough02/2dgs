using System.IO;
using FontStashSharp;

namespace _2dgs;

public class FontManager
{
    private FontSystem _orbitronLightFontSystem;
    private FontSystem _orbitronMediumFontSystem;
    private FontSystem _orbitronBoldFontSystem;
    private FontSystem _orbitronBlackFontSystem;

    public FontManager()
    {
        _orbitronLightFontSystem = new FontSystem();
        _orbitronLightFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_light.ttf"));
        _orbitronMediumFontSystem = new FontSystem();
        _orbitronMediumFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_medium.ttf"));
        _orbitronBoldFontSystem = new FontSystem();
        _orbitronBoldFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_bold.ttf"));
        _orbitronBlackFontSystem = new FontSystem();
        _orbitronBlackFontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_black.ttf"));
    }

    public SpriteFontBase GetOrbitronLightFont(int size)
    {
        return _orbitronLightFontSystem.GetFont(size);
    }

    public SpriteFontBase GetOrbitronMediumFont(int size)
    {
        return _orbitronMediumFontSystem.GetFont(size);
    }

    public SpriteFontBase GetOrbitronBoldFont(int size)
    {
        return _orbitronBoldFontSystem.GetFont(size);
    }

    public SpriteFontBase GetOrbitronBlackFont(int size)
    {
        return _orbitronBlackFontSystem.GetFont(size);
    }
}