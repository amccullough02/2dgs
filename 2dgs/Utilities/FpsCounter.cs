using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class FpsCounter
{
    private bool _showFps = true;
    private double _elapsedTime;
    private int _frameCount;
    private int _fps;
    private const int FontSize = 24;

    public void Update(GameTime gameTime)
    {
        if (!_showFps) return;
        _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameCount++;

        if (_elapsedTime >= 1.0f)
        {
            _fps = _frameCount;
            _frameCount = 0;
            _elapsedTime = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_showFps)
        {
            spriteBatch.Begin();
            var fpsText = $"FPS: {_fps}";
            FontManager.MediumFont(FontSize).
                DrawText(spriteBatch, fpsText, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }

    public void ToggleFps()
    {
        _showFps = !_showFps;
    }
}