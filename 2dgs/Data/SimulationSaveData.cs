using System.Collections.Generic;

namespace _2dgs;

public class SimulationSaveData
{
    public string Title = "default";
    public string Description = "A user generated simulation.";
    public string ThumbnailPath = "../../../savedata/thumbnails/default.png";
    public bool IsLesson = false;
    public int DefaultTimestep = 50;
    public List<BodyData> Bodies = [];
    public List<LessonPage> LessonPages = [];
}