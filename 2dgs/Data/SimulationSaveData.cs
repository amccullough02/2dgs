using System.Collections.Generic;

namespace _2dgs;

public class SimulationSaveData
{
    public string Title = "default";
    public string Description = "A user generated simulation.";
    public bool IsLesson = false;
    public List<BodyData> Bodies = [];
    public List<LessonPage> LessonPages = [];
}