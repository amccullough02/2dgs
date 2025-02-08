using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class SimulationMenuScene(Game game) : Scene
{
    private readonly SimulationMenuUi _simulationMenuUi = new(game);

    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _simulationMenuUi.Draw();
    }
}