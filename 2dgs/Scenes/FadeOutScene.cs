using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class FadeOutScene(Game game) : Scene
{
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    private bool _active = true;
    private float _fadeValue = 1.0f;
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;
    
    public override void Update(GameTime gameTime)
    {
        if (_fadeValue > 0f) _fadeValue -= (float)gameTime.ElapsedGameTime.TotalSeconds * GlobalGameData.FadeSpeed;
        else _active = false;
        if (!_active) game.SceneManager.PopScene();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.BaseTexture, Vector2.Zero, null, Color.Black * _fadeValue, 0f, Vector2.Zero,
            new Vector2(ScreenWidth, ScreenHeight), SpriteEffects.None, 0f);
        spriteBatch.End();
    }
}