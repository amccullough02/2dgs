using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationUi
{
    private readonly Desktop _desktop;
    
    public SimulationUi(Game game, SimulationData simulationData)
    {
        MyraEnvironment.Game = game;
        _desktop = new Desktop();
        var rootContainer = new Panel();
        
        rootContainer.Widgets.Add(SettingsPanel.Create(simulationData));
        rootContainer.Widgets.Add(EditModePanel.Create(simulationData, _desktop));
        rootContainer.Widgets.Add(SaveQuitPanel.Create(simulationData, game, _desktop));
        
        _desktop.Root = rootContainer;
    }

    public Widget GetRoot()
    {
        return _desktop.Root;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}