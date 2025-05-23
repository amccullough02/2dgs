﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A scene used to create a 'fade from black' transition effect.
/// </summary>
/// <param name="game">A reference to the MonoGame Game instance.</param>
public class FadeOutScene(Game game) : Scene
{
    /// <summary>
    /// An instance of TextureManager, used to obtain the scene's background and gradient textures.
    /// </summary>
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    /// <summary>
    /// Used to track if the transition is currently fading from black.
    /// </summary>
    private bool _active = true;
    /// <summary>
    /// Represents the alpha value of the transition element.
    /// </summary>
    private float _fadeValue = 1.0f;
    /// <summary>
    /// A reference to the current width of the screen.
    /// </summary>
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    /// <summary>
    /// A reference to the current height of the screen.
    /// </summary>
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;
    
    /// <summary>
    /// The update method for the FadeInScene class.
    /// </summary>
    /// <param name="gameTime">A reference to MonoGame's GameTime class.</param>
    public override void Update(GameTime gameTime)
    {
        if (_fadeValue > 0f) _fadeValue -= (float)gameTime.ElapsedGameTime.TotalSeconds * GlobalGameData.FadeSpeed;
        else _active = false;
        if (!_active) game.SceneManager.PopScene();
    }

    /// <summary>
    /// The draw method for the 
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="spriteBatch"></param>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.BaseTexture, Vector2.Zero, null, Color.Black * _fadeValue, 0f, Vector2.Zero,
            new Vector2(ScreenWidth, ScreenHeight), SpriteEffects.None, 0f);
        spriteBatch.End();
    }
}