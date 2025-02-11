using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace _2dgs;

public class FadeInScene(Game game, Scene destinationScene) : Scene
{
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    private bool _active = true;
    private float _fadeValue;
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;
    
    public override void Update(GameTime gameTime)
    {
        if (_fadeValue < 1f) _fadeValue += (float)gameTime.ElapsedGameTime.TotalSeconds * GlobalGameData.FadeSpeed;
        else _active = false;
        
        if (!_active)
        {
            game.SceneManager.ClearScenes();
            game.SceneManager.PushScene(destinationScene);
            game.SceneManager.PushScene(new FadeOutScene(game));
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.BaseTexture, Vector2.Zero, null, Color.Black * _fadeValue, 0f, Vector2.Zero,
            new Vector2(ScreenWidth, ScreenHeight), SpriteEffects.None, 0f);
        spriteBatch.End();
    }
}