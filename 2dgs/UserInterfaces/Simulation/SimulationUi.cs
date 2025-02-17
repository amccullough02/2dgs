using Myra;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationUi
{
    private readonly Desktop _desktop;
    
    public SimulationUi(Game game, SimulationMediator simulationMediator)
    {
        MyraEnvironment.Game = game;
        _desktop = new Desktop();
        var rootContainer = new Panel();
        
        rootContainer.Widgets.Add(SettingsPanel.Create(simulationMediator));
        rootContainer.Widgets.Add(EditModePanel.Create(simulationMediator, _desktop));
        rootContainer.Widgets.Add(SaveQuitPanel.Create(simulationMediator, game, _desktop));
        
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