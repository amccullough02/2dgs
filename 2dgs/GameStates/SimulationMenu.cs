using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationMenu : GameState
{
    private SimulationMenuUi _simulationMenuUi;

    public SimulationMenu(Game game)
    {
        _simulationMenuUi = new SimulationMenuUi(game);
    }

    public override void Update(GameTime gameTime) {}

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        _simulationMenuUi.Draw();
    }
}