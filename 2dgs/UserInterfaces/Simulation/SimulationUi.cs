using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A class used to contain UI boilerplate for the Simulation Scene.
/// </summary>
public class SimulationUi
{
    /// <summary>
    /// An instance of a Myra Desktop, the highest unit of organisation in Myra's UI system.
    /// </summary>
    private readonly Desktop _desktop;
    
    /// <summary>
    /// The constructor for the SimulationUi class.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    public SimulationUi(Game game, SimulationMediator simulationMediator)
    {
        MyraEnvironment.Game = game;
        _desktop = new Desktop();
        var rootContainer = new Panel();
        
        rootContainer.Widgets.Add(SettingsPanel.Create(simulationMediator));
        rootContainer.Widgets.Add(EditModePanel.Create(simulationMediator, _desktop));
        rootContainer.Widgets.Add(SaveQuitPanel.Create(game, simulationMediator, _desktop));
        
        _desktop.Root = rootContainer;
    }

    /// <summary>
    /// A helper method to obtain the root widget of the UI, useful for the FindWidget class.
    /// </summary>
    /// <returns>The root widget of the UI.</returns>
    public Widget GetRoot()
    {
        return _desktop.Root;
    }

    /// <summary>
    /// Draws the Myra desktop.
    /// </summary>
    public void Draw()
    {
        _desktop.Render();
    }
}