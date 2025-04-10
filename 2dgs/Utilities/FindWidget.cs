using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace _2dgs;

/// <summary>
/// A class containing various methods for finding, enabling, disabling, and highlighting Myra widgets by their ID.
/// </summary>
public static class FindWidget
{
    /// <summary>
    /// A method to return a widget given an ID. This method will recursively search through the widget tree until either a widget is found or a null is
    /// returned indicating that no such widget exists.
    /// </summary>
    /// <param name="parent">The root widget, typically from an Ui class.</param>
    /// <param name="widgetId">The ID used to search for the widget.</param>
    /// <returns>The Widget with the specified ID or null if a widget with the requested ID does not exist.</returns>
    public static Widget GetWidgetById(Widget parent, string widgetId)
    {
        if (parent.Id == widgetId)
        {
            return parent;
        }

        if (parent is Container parentContainer)
        {
            foreach (var child in parentContainer.GetChildren())
            {
                var widget = GetWidgetById(child, widgetId);

                if (widget != null)
                {
                    return widget;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// A method that highlights a widget given an ID.
    /// </summary>
    /// <param name="parent">The root widget, typically from an Ui class.</param>
    /// <param name="widgetId">The ID used to search for the widget.</param>
    public static void HighlightWidget(Widget parent, string widgetId)
    {
        var widget = GetWidgetById(parent, widgetId);
        if (widget != null)
        {
            widget.BorderThickness = new Thickness(4);
            widget.Border = new SolidBrush(Color.White);
        }
    }

    /// <summary>
    /// A method that removes a highlight from a widget given an ID.
    /// </summary>
    /// <param name="parent">The root widget, typically from an Ui class.</param>
    /// <param name="widgetId">The ID used to search for the widget.</param>
    public static void UnhighlightWidget(Widget parent, string widgetId)
    {
        var widget = GetWidgetById(parent, widgetId);

        if (widget != null)
        {
            widget.BorderThickness = default;
            widget.Border = null;
        }
    }

    /// <summary>
    /// A method to disable a widget or widgets.
    /// </summary>
    /// <param name="parent">The root widget, typically from an Ui class.</param>
    /// <param name="widgetIds">An array of widget IDs.</param>
    public static void DisableWidgets(Widget parent, string[] widgetIds)
    {
        foreach (var widgetId in widgetIds)
        {
            var widget = GetWidgetById(parent, widgetId);
            
            if (widget != null)
            {
                widget.Enabled = false;
            }
        }
    }
    
    /// <summary>
    /// A method to enable a widget or widgets.
    /// </summary>
    /// <param name="parent">The root widget, typically from an Ui class.</param>
    /// <param name="widgetIds">An array of widget IDs.</param>
    public static void EnableWidgets(Widget parent, string[] widgetIds)
    {
        foreach (var widgetId in widgetIds)
        {
            var widget = GetWidgetById(parent, widgetId);
            
            if (widget != null)
            {
                widget.Enabled = true;
            }
        }
    }
}