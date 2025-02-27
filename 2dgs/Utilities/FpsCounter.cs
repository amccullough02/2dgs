using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A class used to display an FPS counter for performance profiling.
/// </summary>
public class FpsCounter
{
    /// <summary>
    /// Controls if the FPS Counter is displayed or not.
    /// </summary>
    private bool _showFps = true;
    /// <summary>
    /// The elapsed time between the previous frame and the current frame.
    /// </summary>
    private double _elapsedTime;
    /// <summary>
    /// The number of frames that have occurred.
    /// </summary>
    private int _frameCount;
    /// <summary>
    /// The current FPS.
    /// </summary>
    private int _fps;
    /// <summary>
    /// The size of the font used to display the FPS counter.
    /// </summary>
    private const int FontSize = 24;

    /// <summary>
    /// Updates the FPS counter.
    /// </summary>
    /// <param name="gameTime">A reference to MonoGame's GameTime class.</param>
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

    /// <summary>
    /// Draws the FPS counter.
    /// </summary>
    /// <param name="spriteBatch">A reference to MonoGame's SpriteBatch class.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        if (_showFps)
        {
            spriteBatch.Begin();
            var fpsText = $"FPS: {_fps}";
            FontManager.MediumText(FontSize).
                DrawText(spriteBatch, fpsText, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }

    /// <summary>
    /// Toggles the FPS counter on/off.
    /// </summary>
    public void ToggleFps()
    {
        _showFps = !_showFps;
    }
}