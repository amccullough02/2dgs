using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class UiComponents
{
     
     public static Button Button(string text,
          bool visible = true,
          int width = UiConstants.DefaultButtonWidth,
          int height = UiConstants.DefaultButtonHeight)
     {
          return new Button
          {
               Width = width,
               Height = height,
               Visible = visible,
               Content = new Label
               {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = text,
                    Font = FontManager.LightFont(UiConstants.DefaultFontSize)
               }
          };
     }

     public static HorizontalSeparator HorizontalSeparator()
     {
          return new HorizontalSeparator
          {
               HorizontalAlignment = HorizontalAlignment.Center,
               VerticalAlignment = VerticalAlignment.Center,
               Thickness = UiConstants.DefaultSeparatorHeight,
               Color = Color.White,
               Width = UiConstants.DefaultElementWidth,
               Margin = new Thickness(0, 10, 0, 10),
          };
     }

     public static HorizontalSlider HorizontalSlider(int value, int min, int max)
     {
          return new HorizontalSlider
          {
               Minimum = min,
               Maximum = max,
               Value = value,
               Width = UiConstants.DefaultElementWidth,
          };
     }

     public static ComboView ComboView()
     {
          return new ComboView()
          {
               Width = UiConstants.DefaultElementWidth,
          };
     }

     public static Label DropdownLabel(string text)
     {
          return new Label
          {
               Text = text,
               HorizontalAlignment = HorizontalAlignment.Center,
               Font = FontManager.LightFont(16),
               Padding = new Thickness(0, 5, 0, 5),
          };
     }

     public static Grid Grid(int spacing, int columns, int rows)
     {
          var grid = new Grid
          {
               RowSpacing = spacing,
               ColumnSpacing = spacing,
               HorizontalAlignment = HorizontalAlignment.Center,
               Margin = new Thickness(10, 10, 10, 10),
          };
          
          for (var i = 0; i < columns; i++)
          {
               grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
          }
          
          for (var i = 0; i < rows; i++)
          {
               grid.RowsProportions.Add(new Proportion(ProportionType.Auto));
          }

          return grid;
     }

     public static Label Label(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.LightFont(UiConstants.DefaultFontSize),
          };
     }
     
     public static Label DialogLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.MediumFont(UiConstants.DialogFontSize),
          };
     }

     public static Label TitleLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.MediumFont(UiConstants.TitleFontSize),
               HorizontalAlignment = HorizontalAlignment.Center,
               Padding = new Thickness(UiConstants.DefaultTitleMargin),
          };
     }

     public static TextBox TextBox(string text)
     {
          return new TextBox
          {
               MinWidth = UiConstants.DefaultTextboxWidth,
               Text = text,
               Font = FontManager.LightFont(UiConstants.DialogFontSize),
          };
     }
     
     public static VerticalStackPanel VerticalStackPanel(int spacing,
          HorizontalAlignment horizontalAlignment,
          VerticalAlignment verticalAlignment, Thickness thickness)
     {
          return new VerticalStackPanel
          {
               Spacing = spacing,
               Margin = thickness,
               HorizontalAlignment = horizontalAlignment,
               VerticalAlignment = verticalAlignment,
          };
     }

     public static ToggleButton ToggleButton(string text, bool toggled)
     {
          return new ToggleButton
          {
               MaxWidth = 150,
               MaxHeight = 40,
               IsToggled = toggled,
               Content = new Label
               {
                    Font = FontManager.LightFont(UiConstants.DefaultFontSize),
                    Text = text,
                    Padding = new Thickness(8),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
               }
          };
     }

     public static Window ValidationWindow(Label label)
     {
          return new Window
          {
               Title = "Validation Error",
               TitleFont = FontManager.MediumFont(UiConstants.DefaultFontSize),
               Content = label
          };
     }

     public static Dialog StyledDialog(string title)
     {
          return new Dialog
          {
               Title = title,
               TitleFont = FontManager.MediumFont(UiConstants.DefaultFontSize),
               Opacity = UiConstants.DefaultDialogOpacity,
               ButtonOk =
               {
                    Content = new Label
                    {
                         Font = FontManager.MediumFont(18),
                         Text = "Ok",
                         Padding = new Thickness(UiConstants.DefaultMargin, 5, UiConstants.DefaultMargin, 5)
                    }
               },
               ButtonCancel =
               {
                    Content = new Label
                    {
                         Font = FontManager.MediumFont(18),
                         Text = "Cancel",
                         Padding = new Thickness(UiConstants.DefaultMargin, 5, UiConstants.DefaultMargin, 5)
                    }
               },
          };
     }

     public static TabControl TabControl(int width)
     {
          return new TabControl
          {
               Width = width,
               MouseCursor = MouseCursorType.Hand,
          };
     }

     public static TabItem TabItem(string title)
     {
          return new TabItem
          {
               Text = title,
          };
     }

     public static ListView ListView(int width)
     {
          return new ListView
          {
               Width = width,
          };
     }
}