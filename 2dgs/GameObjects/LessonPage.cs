namespace _2dgs;

/// <summary>
/// A class used to represent a page of a lesson.
/// </summary>
public class LessonPage
{
    /// <summary>
    /// The text content of the lesson.
    /// </summary>
    public string Text = "";
    /// <summary>
    /// The widget to highlight.
    /// </summary>
    public string HighlightWidget = "";
    /// <summary>
    /// The widgets to disable.
    /// </summary>
    public string[] RestrictWidgets = [];
}