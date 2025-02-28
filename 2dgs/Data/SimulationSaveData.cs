using System.Collections.Generic;

namespace _2dgs;

/// <summary>
/// A class used to represent the loaded data from a simulation file.
/// </summary>
public class SimulationSaveData
{
    /// <summary>
    /// The title of the simulation, this is shown in the lesson prompt. Non-lesson sims will use 'default' as they do not use a lesson prompt.
    /// </summary>
    public string Title = "default";
    /// <summary>
    /// The description of the simulation that is seen in the simulation menu. Non-lesson sims will use 'A user generated simulation.', whilst lesson simulations
    /// will have a custom description outlining the physical concept therein .
    /// </summary>
    public string Description = "A user generated simulation.";
    /// <summary>
    /// The path to the thumbnail displayed in the simulation menu. A default thumbnail is used for non-lesson sims, whilst lesson sims will have bespoke a
    /// bespoke thumbnail set in their JSON files.
    /// </summary>
    public string ThumbnailPath = "../../../savedata/thumbnails/default.png";
    /// <summary>
    /// Indicates if the simulation is a lesson or sandbox simulation (if false, the lesson prompt will be hidden; if true, the save button is hidden).
    /// </summary>
    public bool IsLesson = false;
    /// <summary>
    /// Specifies a default timestep for the simulation, this will be set to a lower value in simulations that demonstrate quick interactions and vice versa.
    /// </summary>
    public int DefaultTimestep = 50;
    /// <summary>
    /// The bodies to be simulated.
    /// </summary>
    public List<BodyData> Bodies = [];
    /// <summary>
    /// The contents of the simulation lesson, if there is one.
    /// </summary>
    public List<LessonPage> LessonPages = [];
}