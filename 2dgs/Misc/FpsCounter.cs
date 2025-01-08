using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class FpsCounter
{
    private bool _showFps;
    private double _elapsedTime;
    private int _frameCount;
    private int _fps;
    private const int FontSize = 24;
    private FontManager _fontManager;

    public FpsCounter()
    {
        _showFps = true;
        _fontManager = new FontManager();
    }

    public void Update(GameTime gameTime)
    {
        if (_showFps)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCount++;

            if (_elapsedTime >= 1.0f)
            {
                _fps = _frameCount;
                _frameCount = 0;
                _elapsedTime = 0;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_showFps)
        {
            spriteBatch.Begin();
            string fpsText = $"FPS: {_fps}";
            _fontManager.BoldFont(FontSize).
                DrawText(spriteBatch, fpsText, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }

    public void ToggleFps()
    {
        _showFps = !_showFps;
    }
}