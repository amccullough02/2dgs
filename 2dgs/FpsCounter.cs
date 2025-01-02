﻿using System.IO;
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
    private const int FontSize = 32;

    private readonly FontSystem _fontSystem;
    private SpriteFontBase _font;

    public FpsCounter()
    {
        _showFps = true;
        _fontSystem = new FontSystem();
        _fontSystem.AddFont(File.ReadAllBytes("../../../assets/fonts/orbitron_bold.ttf"));
        _font = _fontSystem.GetFont(FontSize);
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
            _font.DrawText(spriteBatch, fpsText, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
    }

    public void ToggleFps()
    {
        _showFps = !_showFps;
    }
}