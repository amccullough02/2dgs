namespace _2dgs;

public class SimulationData
{
    public bool IsPaused { get; set; }
    public bool ToggleTrails { get; set; } = true;
    public bool ToggleNames { get; set; } = true;
    public bool ToggleBodyGhost { get; set; }
    public int TimeStep { get; set; } = 1;
    public int TrailLength { get; set; } = 250;
    public Position Position { get; set; } = Position.Left;
}