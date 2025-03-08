using Microsoft.Xna.Framework;
using Myra.Graphics2D;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.ColorPicker;

namespace _2dgs;

/// <summary>
/// A basic UiComponents factory class. These are Myra components with additional defaults that are specific to 2DGS.
/// </summary>
public static class UiComponents
{
     /// <summary>
     /// A method to create a standard 2DGS button.
     /// </summary>
     /// <param name="text">The text of the button.</param>
     /// <param name="visible">The button's visibility.</param>
     /// <param name="width">The pixel width of the button.</param>
     /// <param name="height">The pixel height of the button.</param>
     /// <returns>A 2DGS button instance.</returns>
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
                    Font = FontManager.LightText(UiConstants.DefaultFontSize)
               }
          };
     }
     
     /// <summary>
     /// A method to create a 2DGS (main) menu button.
     /// </summary>
     /// <param name="text">The text of the button.</param>
     /// <param name="visible">The button's visibility.</param>
     /// <param name="width">The pixel width of the button.</param>
     /// <param name="height">The pixel height of the button.</param>
     /// <returns>A 2DGS (main) menu button instance.</returns>
     public static Button MenuButton(string text,
          bool visible = true,
          int width = UiConstants.DefaultButtonWidth,
          int height = UiConstants.DefaultButtonHeight)
     {
          return new Button
          {
               Width = width,
               Height = height,
               Visible = visible,
               Background = new SolidBrush(Color.Black * 0.66f),
               PressedBackground = new SolidBrush(Color.DarkBlue * 0.33f),
               OverBackground = new SolidBrush(Color.Cyan * 0.33f),
               Border = new SolidBrush(Color.White),
               BorderThickness = new Thickness(1),
               Content = new Label
               {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = text,
                    Font = FontManager.ButtonText(UiConstants.DefaultMainMenuFontSize)
               }
          };
     }

     /// <summary>
     /// A method to create a 2DGS horizontal separator.
     /// </summary>
     /// <param name="width">The pixel width of the button.</param>
     /// <param name="height">The pixel height of the button.</param>
     /// <param name="margin">The margin for the separator.</param>
     /// <returns>A 2DGS horizontal separator instance.</returns>
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

     /// <summary>
     /// A method to create a 2DGS horizontal slider.
     /// </summary>
     /// <param name="value">The default value for the slider.</param>
     /// <param name="min">The minimum value for the slider.</param>
     /// <param name="max">The maximum value for the slider.</param>
     /// <returns>A 2DGS horizontal slider instance.</returns>
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

     /// <summary>
     /// A method to create a 2DGS combo view.
     /// </summary>
     /// <param name="width">The width of the combo view.</param>
     /// <returns>A 2DGS combo view instance.</returns>
     public static ComboView ComboView(int width = UiConstants.DefaultElementWidth)
     {
          return new ComboView()
          {
               Width = width,
          };
     }

     /// <summary>
     /// A method to create a 2DGS dropdown label.
     /// </summary>
     /// <param name="text">The label's text.</param>
     /// <returns>A 2DGS dropdown label instance.</returns>
     public static Label DropdownLabel(string text)
     {
          return new Label
          {
               Text = text,
               HorizontalAlignment = HorizontalAlignment.Center,
               Font = FontManager.LightText(16),
               Padding = new Thickness(0, 5, 0, 5),
          };
     }

     /// <summary>
     /// A method to create a 2DGS widget grid.
     /// </summary>
     /// <param name="spacing">The spacing between grid elements.</param>
     /// <param name="columns">The number of columns the grid will have.</param>
     /// <param name="rows">The number of rows the grid will have.</param>
     /// <returns></returns>
     public static Grid Grid(int spacing, int columns, int rows)
     {
          var grid = new Grid
          {
               RowSpacing = spacing,
               ColumnSpacing = spacing,
               HorizontalAlignment = HorizontalAlignment.Center,
               Margin = new Thickness(10),
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

     /// <summary>
     /// A method to create a 2DGS key bind preview label.
     /// </summary>
     /// <param name="text">The text of the label.</param>
     /// <returns>A 2DGS key bind preview lable instance.</returns>
     public static Label KeyBindLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.LightText(UiConstants.DefaultDialogFontSize),
               VerticalAlignment = VerticalAlignment.Center,
               Border = new SolidBrush(Color.White),
               BorderThickness = new Thickness(1),
               Padding = new Thickness(4)
          };
     }

     /// <summary>
     /// A method to create a 2DGS light label.
     /// </summary>
     /// <param name="text">The text of the label.</param>
     /// <param name="fontSize">The font size of the label.</param>
     /// <returns>A 2DGS light label.</returns>
     public static Label LightLabel(string text, int fontSize = UiConstants.DefaultFontSize)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.LightText(fontSize),
               VerticalAlignment = VerticalAlignment.Center,
          };
     }
     
     /// <summary>
     /// A method to create a 2DGS medium label.
     /// </summary>
     /// <param name="text">The text of the label.</param>
     /// <param name="fontSize">The font size of the label.</param>
     /// <returns>A 2DGS medium label.</returns>
     public static Label MediumLabel(string text, int fontSize = UiConstants.DefaultFontSize)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.MediumText(fontSize),
               VerticalAlignment = VerticalAlignment.Center,
          };
     }

     /// <summary>
     /// A method to create a 2DGS title label.
     /// </summary>
     /// <param name="text">The text of the label.</param>
     /// <returns>A 2DGS title label.</returns>
     public static Label TitleLabel(string text)
     {
          return new Label
          {
               Text = text,
               Font = FontManager.TitleText(UiConstants.DefaultTitleFontSize),
               HorizontalAlignment = HorizontalAlignment.Center,
               Padding = new Thickness(UiConstants.DefaultTitleMargin),
          };
     }

     /// <summary>
     /// A method to create a 2DGS text box.
     /// </summary>
     /// <param name="text">The text box's text.</param>
     /// <returns>A 2DGS text box.</returns>
     public static TextBox TextBox(string text)
     {
          return new TextBox
          {
               MinWidth = UiConstants.DefaultTextboxWidth,
               Text = text,
               Font = FontManager.LightText(UiConstants.DefaultDialogFontSize),
               Padding = new Thickness(4),
               BorderThickness = new Thickness(1),
               Border = new SolidBrush(Color.Gray)
          };
     }

     /// <summary>
     /// A method to create a 2DGS toggle button.
     /// </summary>
     /// <param name="text">The text of the toggle button.</param>
     /// <param name="toggled">Is the toggle button enabled or not.</param>
     /// <returns>A 2DGS toggle button.</returns>
     public static ToggleButton ToggleButton(string text, bool toggled)
     {
          return new ToggleButton
          {
               MaxWidth = 150,
               MaxHeight = 40,
               IsToggled = toggled,
               Content = new Label
               {
                    Font = FontManager.LightText(UiConstants.DefaultFontSize),
                    Text = text,
                    Padding = new Thickness(UiConstants.DefaultMediumPadding),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
               }
          };
     }

     /// <summary>
     /// A method to create a 2DGS validation error window.
     /// </summary>
     /// <param name="label">The label the window will contain.</param>
     /// <returns>A 2DGS validation error window.</returns>
     public static Window ValidationWindow(Label label)
     {
          return new Window
          {
               Title = "Validation Error",
               TitleFont = FontManager.MediumText(UiConstants.DefaultFontSize),
               Content = label
          };
     }

     /// <summary>
     /// A method to create a 2DGS dialog.
     /// </summary>
     /// <param name="title">The title of the dialog.</param>
     /// <returns>A 2DGS dialog.</returns>
     public static Dialog StyledDialog(string title)
     {
          return new Dialog
          {
               Title = title,
               TitleFont = FontManager.MediumText(UiConstants.DefaultFontSize),
               Background = new SolidBrush(Color.Black * UiConstants.DefaultDialogOpacity),
               BorderThickness = new Thickness(1),
               Border = new SolidBrush(Color.White),
               Padding = new Thickness(UiConstants.DefaultMediumPadding),
               TitlePanel =
               {
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    Padding = new Thickness(UiConstants.DefaultSmallPadding)
               },
               ButtonOk =
               {
                    Content = new Label
                    {
                         Font = FontManager.MediumText(18),
                         Text = "Confirm",
                         Padding = new Thickness(UiConstants.DefaultSmallPadding)
                    }
               },
               ButtonCancel =
               {
                    Content = new Label
                    {
                         Font = FontManager.MediumText(18),
                         Text = "Cancel",
                         Padding = new Thickness(UiConstants.DefaultSmallPadding)
                    }
               },
          };
     }

     /// <summary>
     /// A method to create a 2DGS color picker dialog.
     /// </summary>
     /// <returns>A 2DGS color picker dialog.</returns>
     public static ColorPickerDialog CustomColorPicker()
     {
          return new ColorPickerDialog
          {
               Title = "Select Body Colour",
               TitleFont = FontManager.MediumText(UiConstants.DefaultFontSize),
               Background = new SolidBrush(Color.Black * UiConstants.DefaultDialogOpacity),
               BorderThickness = new Thickness(1),
               Border = new SolidBrush(Color.White),
               Padding = new Thickness(UiConstants.DefaultMediumPadding),
               TitlePanel =
               {
                    HorizontalAlignment = HorizontalAlignment.Center, 
                    Padding = new Thickness(UiConstants.DefaultSmallPadding)
               },
               ButtonOk =
               {
                    Content = new Label
                    {
                         Font = FontManager.MediumText(18),
                         Text = "Confirm",
                         Padding = new Thickness(UiConstants.DefaultSmallPadding)
                    }
               },
               ButtonCancel =
               {
                    Content = new Label
                    {
                         Font = FontManager.MediumText(18),
                         Text = "Cancel",
                         Padding = new Thickness(UiConstants.DefaultSmallPadding)
                    }
               },
          };
     }

     /// <summary>
     /// A method to create a 2DGS (read-only) text box.
     /// </summary>
     /// <param name="text">The text box's text.</param>
     /// <returns>A 2DGS (read-only) text box.</returns>
     public static TextBox ReadOnlyTextBox(string text)
     {
          return new TextBox
          {
               Text = text,
               Font = FontManager.LightText(UiConstants.DefaultFontSize),
               Padding = new Thickness(UiConstants.DefaultLargePadding),
               Multiline = true,
               Readonly = true,
               Wrap = true,
               HorizontalAlignment = HorizontalAlignment.Stretch,
               VerticalAlignment = VerticalAlignment.Stretch,
               BlinkIntervalInMs = 2000,
               Background = new SolidBrush(new Color(32, 32, 32) * UiConstants.DefaultLessonOpacity),
               BorderThickness = new Thickness(1),
               Border = new SolidBrush(Color.Gray)
          };
     }

     /// <summary>
     /// A method to create a 2DGS tab control.
     /// </summary>
     /// <param name="width">The pixel width of the widget.</param>
     /// <param name="height">The pixel height of the widget.</param>
     /// <returns>A 2DGS tab control widget.</returns>
     public static TabControl TabControl(int width, int height)
     {
          return new TabControl
          {
               Width = width,
               Height = height,
               MouseCursor = MouseCursorType.Hand,
          };
     }

     /// <summary>
     /// A method to create a 2DGS tab item.
     /// </summary>
     /// <param name="title">The title of the tab item.</param>
     /// <returns>A 2DGS tab item.</returns>
     public static TabItem TabItem(string title)
     {
          return new TabItem
          {
               Text = title,
          };
     }

     /// <summary>
     /// A method to create a 2DGS list view.
     /// </summary>
     /// <param name="width">The pixel width of the list view.</param>
     /// <returns>A 2DGS list view.</returns>
     public static ListView ListView(int width)
     {
          return new ListView
          {
               Width = width,
               Margin = new Thickness(10),
          };
     }
}