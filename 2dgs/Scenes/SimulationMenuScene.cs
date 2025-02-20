using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class SimulationMenuScene(Game game) : Scene
{
    private readonly SimulationMenuUi _simulationMenuUi = new(game);
    private readonly TextureManager _textureManager = new(game.Content, game.GraphicsDevice);
    private float ScreenWidth => game.GraphicsDevice.Viewport.Width;
    private float ScreenHeight => game.GraphicsDevice.Viewport.Height;

    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_textureManager.SimulationMenuBackground, _textureManager.PositionAtCenter(ScreenWidth, 
            ScreenHeight, _textureManager.SimulationMenuBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            _textureManager.PositionAtCenter(ScreenWidth, ScreenHeight, _textureManager.Gradient), Color.White);
        spriteBatch.End();
        _simulationMenuUi.Draw();
    }
}