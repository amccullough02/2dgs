using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
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

     public static HorizontalSeparator HorizontalSeparator(int width=UiConstants.DefaultElementWidth, int height=UiConstants.DefaultSeparatorHeight, int margin=10)
     {
          return new HorizontalSeparator
          {
               HorizontalAlignment = HorizontalAlignment.Center,
               VerticalAlignment = VerticalAlignment.Center,
               Thickness = height,
               Color = Color.White,
               Width = width,
               Margin = new Thickness(0, margin, 0, margin),
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

     public static ComboView ComboView(int width = UiConstants.DefaultElementWidth)
     {
          return new ComboView()
          {
               Width = width,
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

     public static Label KeyBindLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.LightFont(UiConstants.DialogFontSize),
               VerticalAlignment = VerticalAlignment.Center,
               Border = new SolidBrush(Color.White),
               BorderThickness = new Thickness(1),
               Padding = new Thickness(4)
          };
     }

     public static Label LightLabel(string text, int fontSize = UiConstants.DefaultFontSize)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.LightFont(fontSize),
               VerticalAlignment = VerticalAlignment.Center,
          };
     }
     
     public static Label MediumLabel(string text, int fontSize = UiConstants.DefaultFontSize)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.MediumFont(fontSize),
               VerticalAlignment = VerticalAlignment.Center,
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
               Padding = new Thickness(4),
               BorderThickness = new Thickness(1),
               Border = new SolidBrush(Color.Gray)
          };
     }
     
     // TODO Serves no real purpose -- remove or provide default values.
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
               TitlePanel =
               {
                    HorizontalAlignment = HorizontalAlignment.Center, Padding = new Thickness(0,
                         5,
                         0,
                         0)
               },
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

     public static TextBox ReadOnlyTextBox(string text)
     {
          return new TextBox
          {
               Text = text,
               Font = FontManager.LightFont(UiConstants.DefaultFontSize),
               Padding = new Thickness(10),
               Multiline = true,
               Readonly = true,
               Wrap = true,
               HorizontalAlignment = HorizontalAlignment.Stretch,
               VerticalAlignment = VerticalAlignment.Stretch,
               BlinkIntervalInMs = 2000,
               Background = new SolidBrush(new Color(32, 32, 32)),
               BorderThickness = new Thickness(1),
               Border = new SolidBrush(Color.Gray)
          };
     }

     public static TabControl TabControl(int width, int height)
     {
          return new TabControl
          {
               Width = width,
               Height = height,
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
               Margin = new Thickness(10),
          };
     }
}