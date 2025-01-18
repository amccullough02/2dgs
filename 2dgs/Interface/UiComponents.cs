using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public static class UiComponents
{
     
     public static Button CreateButton(string text,
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

     public static HorizontalSeparator CreateHorizontalSeparator()
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

     public static HorizontalSlider CreateHorizontalSlider(int value, int min, int max)
     {
          return new HorizontalSlider
          {
               Minimum = min,
               Maximum = max,
               Value = value,
               Width = UiConstants.DefaultElementWidth,
          };
     }

     public static ComboView CreateComboView()
     {
          return new ComboView()
          {
               Width = UiConstants.DefaultElementWidth,
               SelectedIndex = 0,
          };
     }

     public static Label CreateDropdownLabel(string text)
     {
          return new Label
          {
               Text = text,
               HorizontalAlignment = HorizontalAlignment.Center,
               Font = FontManager.LightFont(16),
               Padding = new Thickness(0, 5, 0, 5),
          };
     }

     public static Grid CreateGrid(int spacing, int columns, int rows)
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

     public static Label CreateStyledLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.LightFont(UiConstants.DefaultFontSize),
          };
     }
     
     public static Label CreateDialogLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.MediumFont(UiConstants.DialogFontSize),
          };
     }

     public static TextBox CreateBasicTextBox(string text)
     {
          return new TextBox
          {
               MinWidth = UiConstants.DefaultTextboxWidth,
               Text = text,
               Font = FontManager.LightFont(UiConstants.DialogFontSize),
          };
     }
     
     public static VerticalStackPanel CreateVerticalStackPanel(int spacing,
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

     public static ToggleButton CreateToggleButton(string text, bool toggled)
     {
          return new ToggleButton
          {
               IsToggled = toggled,
               Content = new Label
               {
                    Text = text
               }
          };
     }

     public static Window CreateValidationWindow(Label label)
     {
          return new Window
          {
               Title = "Validation Error",
               TitleFont = FontManager.MediumFont(UiConstants.DefaultFontSize),
               Content = label
          };
     }

     public static Dialog CreateStyledDialog(string title)
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
}