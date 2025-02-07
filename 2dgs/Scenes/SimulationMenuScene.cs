using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class SimulationMenuScene : Scene
{
    private readonly SimulationMenuUi _simulationMenuUi;
    private readonly SaveSystem _saveSystem = new();

    public SimulationMenuScene(Game game)
    {
        _simulationMenuUi = new SimulationMenuUi(game, _saveSystem);
    }

    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _simulationMenuUi.Draw();
    }
}