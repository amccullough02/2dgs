using Microsoft.Xna.Framework;

namespace _2dgs;

public class SimulationData
{
    public bool IsPaused { get; set; }
    public bool ToggleTrails { get; set; } = true;
    public bool ToggleNames { get; set; } = true;
    public bool ToggleBodyGhost { get; set; }
    public bool IsABodySelected { get; set; }
    public bool ColorSelectedBody { get; set; }
    public bool EditSelectedBody { get; set; }
    public bool DeleteSelectedBody { get; set; }
    public bool AttemptToSaveFile { get; set; }
    public bool EditMode { get; set; }
    public bool IsLesson { get; set; }
    public string FilePath { get; set; }
    public string SimulationTitle { get; set; }
    public string[] LessonContent { get; set; }
    public int TimeStep { get; set; } = 50;
    public int TrailLength { get; set; } = 250;
    public Color NewBodyColor { get; set; }
    public Position Position { get; set; } = Position.Right;
    public BodyData SelectedBodyData { get; set; } = new();
    public BodyData CreateBodyData { get; set; } = new();
    public BodyData EditBodyData { get; set; } = new();
}