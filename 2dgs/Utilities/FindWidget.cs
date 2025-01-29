using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class FindWidget
{
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
                Widget widget = GetWidgetById(child, widgetId);

                if (widget != null)
                {
                    return widget;
                }
            }
        }
        return null;
    }

    public static void HighlightWidget(Widget parent, string widgetId)
    {
        var widget = GetWidgetById(parent, widgetId);
        if (widget != null)
        {
            widget.BorderThickness = new Thickness(4);
            widget.Border = new SolidBrush(Color.White);
        }
    }

    public static void UnhighlightWidget(Widget parent, string widgetId)
    {
        var widget = GetWidgetById(parent, widgetId);

        if (widget != null)
        {
            widget.BorderThickness = default;
            widget.Border = null;
        }
    }

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