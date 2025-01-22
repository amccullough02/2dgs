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
}