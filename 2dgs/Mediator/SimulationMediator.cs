using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace _2dgs;

/// <summary>
/// A mediator class that manages communication between the simulation scene, simulation user interface, body class, and simulation save file.
/// </summary>
public class SimulationMediator
{
    /// <summary>
    /// Signifies if the simulation is paused.
    /// </summary>
    public bool Paused { get; set; }
    /// <summary>
    /// Signifies if body trails are showing.
    /// </summary>
    public bool ToggleTrails { get; set; } = true;
    /// <summary>
    /// Signifies if body orbits are showing.
    /// </summary>
    public bool ToggleOrbits { get; set; }
    /// <summary>
    /// Signifies if body vectors are showing.
    /// </summary>
    public bool ToggleVectors { get; set; }
    /// <summary>
    /// Signifies if body names are showing.
    /// </summary>
    public bool ToggleNames { get; set; } = true;
    /// <summary>
    /// Signifies if the bodies are glowing.
    /// </summary>
    public bool ToggleGlow { get; set; }
    /// <summary>
    /// Signifies if a ghost body is to be rendered (set to true when a new body is created but not placed yet).
    /// </summary>
    public bool ToggleBodyGhost { get; set; }
    /// <summary>
    /// Signifies if a body is selected (useful to preventing multiple selections which are not yet supported).
    /// </summary>
    public bool ABodySelected { get; set; }
    /// <summary>
    /// Signifies if the selected body is to have its color updated.
    /// </summary>
    public bool ColorSelectedBody { get; set; }
    /// <summary>
    /// Signifies if the selected body is to be edited.
    /// </summary>
    public bool EditSelectedBody { get; set; }
    /// <summary>
    /// Signifies if the selected body is to be deleted.
    /// </summary>
    public bool DeleteSelectedBody { get; set; }
    /// <summary>
    /// Signifies if the simulation should be saved to a file.
    /// </summary>
    public bool AttemptToSaveFile { get; set; }
    /// <summary>
    /// Signifies if edit mode is enabled or not.
    /// </summary>
    public bool EditMode { get; set; }
    /// <summary>
    /// Signifies if the simulation is a lesson simulation (otherwise the lesson features are disabled and saving is allowed).
    /// </summary>
    public bool Lesson { get; set; }
    /// <summary>
    /// Signifies if the simulation should be reset to its initial state.
    /// </summary>
    public bool ResetSimulation { get; set; }
    /// <summary>
    /// The file path of the simulation file the simulation has been loaded from (blank if the simulation is newly created).
    /// </summary>
    public string FilePath { get; set; }
    /// <summary>
    /// The 'title' of the simulation (this is what displays in the lesson prompt window).
    /// </summary>
    public string SimulationTitle { get; set; }
    /// <summary>
    /// The current time step of the simulation which controls how the rate at which orbits progress.
    /// </summary>
    public int TimeStep { get; set; } = 50;
    /// <summary>
    /// A value used to aid in drawing vector arrows of a reasonable length per lesson.
    /// </summary>
    public int VectorMultiplier { get; set; } = 15;
    /// <summary>
    /// The current length of body trails.
    /// </summary>
    public int TrailLength { get; set; } = 250;
    /// <summary>
    /// The current screen dimensions (used to offset bodies from relative to (0,0) to the centre of the screen.
    /// </summary>
    public Vector2 ScreenDimensions { get; set; }
    /// <summary>
    /// The colour the user to apply to a new body.
    /// </summary>
    public Color NewBodyColor { get; set; }
    /// <summary>
    /// The position of body names relative to the bodies proper.
    /// </summary>
    public Position Position { get; set; } = Position.Right;
    /// <summary>
    /// A list of lesson pages (used in lesson simulations).
    /// </summary>
    public List<LessonPage> LessonPages { get; set; } = [];
    /// <summary>
    /// A reference to the data on the currently selected body (used to provide a starting point for edits).
    /// </summary>
    public BodyData SelectedBodyData { get; set; } = new();
    /// <summary>
    /// Contains the data the user has provided to create a new body.
    /// </summary>
    public BodyData CreateBodyData { get; set; } = new();
    /// <summary>
    /// Contains the data the user has provided to edit a body.
    /// </summary>
    public BodyData EditBodyData { get; set; } = new();
}