namespace _2dgs;

public class SimulationData
{
    public bool IsPaused { get; set; }
    public bool ToggleTrails { get; set; } = true;
    public bool ToggleNames { get; set; } = true;
    public bool ToggleBodyGhost { get; set; }
    public bool IsABodySelected { get; set; }
    public bool EditSelectedBody { get; set; }
    public bool DeleteSelectedBody { get; set; }
    public bool AttemptToSaveFile { get; set; }
    public bool EditMode { get; set; }
    public string FilePath { get; set; }
    public int TimeStep { get; set; } = 1;
    public int TrailLength { get; set; } = 250;
    public Position Position { get; set; } = Position.Left;
    public BodyData CreateBodyData { get; set; } = new();
    public BodyData EditBodyData { get; set; } = new();
}