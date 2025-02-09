using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2dgs;

public class SimulationSceneData
{
    public bool Paused { get; set; }
    public bool ToggleTrails { get; set; } = true;
    public bool ToggleOrbits { get; set; }
    public bool ToggleNames { get; set; } = true;
    public bool ToggleGlow { get; set; }
    public bool ToggleBodyGhost { get; set; }
    public bool ABodySelected { get; set; }
    public bool ColorSelectedBody { get; set; }
    public bool EditSelectedBody { get; set; }
    public bool DeleteSelectedBody { get; set; }
    public bool AttemptToSaveFile { get; set; }
    public bool EditMode { get; set; }
    public bool Lesson { get; set; }
    public bool ResetSimulation { get; set; }
    public string FilePath { get; set; }
    public string SimulationTitle { get; set; }
    public int TimeStep { get; set; } = 50;
    public int TrailLength { get; set; } = 250;
    public Vector2 ScreenDimensions { get; set; }
    public Color NewBodyColor { get; set; }
    public Position Position { get; set; } = Position.Right;
    public List<LessonPage> LessonPages { get; set; } = [];
    public BodyData SelectedBodyData { get; set; } = new();
    public BodyData CreateBodyData { get; set; } = new();
    public BodyData EditBodyData { get; set; } = new();
}