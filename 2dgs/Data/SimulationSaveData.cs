using System.Collections.Generic;

namespace _2dgs;

public class SimulationSaveData
{
    public string Title = "default";
    public bool IsLesson = false;
    public List<BodyData> Bodies = [];
    public List<LessonPage> LessonPages = [];
}