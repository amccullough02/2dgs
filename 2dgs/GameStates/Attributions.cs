using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Attributions(Game game) : GameState
{
    private readonly AttributionsUi _attributionsUi = new(game);
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;
    
    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.AttributionsBackground, _textureManager.PositionAtCenter(ScreenWidth, ScreenHeight, 
            _textureManager.MainMenuBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            _textureManager.PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), Color.White);
        spriteBatch.End();
        _attributionsUi.Draw();
    }
}