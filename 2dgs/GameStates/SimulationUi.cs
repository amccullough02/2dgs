using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationUi
{
    private Game _game;
    private Desktop _desktop;
    private FontManager _fontManager;
    private bool _wasKeyPreviouslyDown;
    
    public SimulationUi(Game game, SimulationData simData)
    {
        _game = game;
        _fontManager = new FontManager();
        
        MyraEnvironment.Game = _game;
        
        var rootContainer = new Panel();
        rootContainer.Widgets.Add(SettingsPanel(simData));
        rootContainer.Widgets.Add(ReturnButton());
        rootContainer.Widgets.Add(EditPanel(simData));
        
        _desktop = new Desktop();
        _desktop.Root = rootContainer;
    }

    private VerticalStackPanel SettingsPanel(SimulationData simData)
    {
        var settingsPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(UIConstants.DefaultMargin, 0, 0, UIConstants.DefaultMargin),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        var timestepLabel = new Label
        {
            Text = $"Time step: {simData.TimeStep}",
            Font = _fontManager.LightFont(UIConstants.DefaultMargin)
        };

        var timestepSlider = new HorizontalSlider
        {
            Minimum = 1,
            Maximum = 10,
            Value = 1,
            Width = UIConstants.DefaultElementWidth,
        };

        timestepSlider.ValueChanged += (s, e) =>
        {
            timestepLabel.Text = $"Time step: {(int)timestepSlider.Value}";
            simData.TimeStep = (int)timestepSlider.Value;
        };
        
        var pauseButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Pause Simulation",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        pauseButton.Click += (s, e) =>
        {
            simData.IsPaused = !simData.IsPaused;
            if (simData.EditMode) simData.EditMode = false;
        };

        var firstDivider = new HorizontalSeparator
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Thickness = UIConstants.DefaultSeparatorHeight,
            Color = Color.White,
            Width = UIConstants.DefaultElementWidth,
            Margin = new Thickness(0, 10, 0, 10),
        };
        
        var trailLengthLabel = new Label
        {
            Text = $"Trail length: {simData.TrailLength}",
            Font = _fontManager.LightFont(UIConstants.DefaultFontSize)
        };

        var trailLengthSlider = new HorizontalSlider
        {
            Minimum = 250,
            Maximum = 2000,
            Value = 1,
            Width = UIConstants.DefaultElementWidth,
        };

        trailLengthSlider.ValueChanged += (s, e) =>
        {
            trailLengthLabel.Text = $"Trail length: {(int)trailLengthSlider.Value}";
            simData.TrailLength = (int)trailLengthSlider.Value;
        };
        
        var trailsButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Toggle Trails",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        trailsButton.Click += (s, e) =>
        {
            simData.ToggleTrails = !simData.ToggleTrails;
        };
        
        var secondDivider = new HorizontalSeparator
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Thickness = UIConstants.DefaultSeparatorHeight,
            Color = Color.White,
            Width = UIConstants.DefaultElementWidth,
            Margin = new Thickness(0, 10, 0, 10),
        };
        
        var namesButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Toggle Names",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        namesButton.Click += (s, e) =>
        {
            simData.ToggleNames = !simData.ToggleNames;
        };

        var namesDropdown = new ComboView()
        {
            Width = UIConstants.DefaultElementWidth,
            SelectedIndex = 0,
        };
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Left",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Right",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Top",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });
        
        namesDropdown.Widgets.Add(new Label
        {
            Text = "Bottom",
            HorizontalAlignment = HorizontalAlignment.Center,
            Font = _fontManager.LightFont(18),
            Padding = new Thickness(0, 5, 0, 5),
        });

        namesDropdown.SelectedIndex = 0;
        namesDropdown.SelectedIndexChanged += (s, e) =>
        {
            switch (namesDropdown.SelectedIndex)
            {
                case 0:
                    simData.Position = Position.Left;
                    break;
                case 1:
                    simData.Position = Position.Right;
                    break;
                case 2:
                    simData.Position = Position.Top;
                    break;
                case 3:
                    simData.Position = Position.Bottom;
                    break;
            }
        };
        
        settingsPanel.Widgets.Add(timestepLabel);
        settingsPanel.Widgets.Add(timestepSlider);
        settingsPanel.Widgets.Add(pauseButton);
        settingsPanel.Widgets.Add(firstDivider);
        settingsPanel.Widgets.Add(trailLengthLabel);
        settingsPanel.Widgets.Add(trailLengthSlider);
        settingsPanel.Widgets.Add(trailsButton);
        settingsPanel.Widgets.Add(secondDivider);
        settingsPanel.Widgets.Add(namesButton);
        settingsPanel.Widgets.Add(namesDropdown);

        return settingsPanel;
    }

    private Dialog CreateBodyDialog(SimulationData simData)
    {
        var grid = new Grid
        {
            RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10, 10, 10, 10),
        };
        
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // NAME
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // VELOCITY X
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // VELOCITY Y
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // MASS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // DISPLAY RADIUS

        // BODY NAME
        var bodyNameLabel = new Label
        {
            Text = "Body Name: ",
        };
        
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);

        var bodyNameTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "Default name"
        };
        
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        
        // VEL X
        var bodyVelXLabel = new Label
        {
            Text = "Body Vel X: ",
        };
        
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 1);

        var bodyVelXTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "0.0",
        };
        
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 1);
        
        // VEL Y
        var bodyVelYLabel = new Label
        {
            Text = "Body Vel Y: ",
        };
        
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 2);

        var bodyVelYTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "4.0",
        };
        
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 2);
        
        // MASS
        var bodyMassLabel = new Label
        {
            Text = "Body Mass: ",
        };
        
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 3);

        var bodyMassTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "1e6",
        };
        
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 3);
        
        // DISPLAY RADIUS
        var bodyDisplaySizeLabel = new Label
        {
            Text = "Body Size: ",
        };
        
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 4);

        var bodyDisplaySizeTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "0.05",
        };
        
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 4);
        
        // DIALOG AND SETUP
        var createBodyDialogue = new Dialog
        {
            Title = "Create New Body",
            Content = grid
        };

        createBodyDialogue.ButtonOk.Click += (sender, e) =>
        {
            string name = bodyNameTextbox.Text;
            Vector2 velocity = new Vector2(float.Parse(bodyVelXTextbox.Text), float.Parse(bodyVelYTextbox.Text));
            float mass = float.Parse(bodyMassTextbox.Text);
            float size = float.Parse(bodyDisplaySizeTextbox.Text);

            simData.CreateBodyData.Name = name;
            simData.CreateBodyData.Velocity = velocity;
            simData.CreateBodyData.Mass = mass;
            simData.CreateBodyData.DisplayRadius = size;
            simData.ToggleBodyGhost = true;
        };

        return createBodyDialogue;
    }

    private Dialog EditBodyDialog(SimulationData simData)
    {
        var grid = new Grid
        {
            RowSpacing = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10, 10, 10, 10),
        };
        
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // NAME
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // VELOCITY X
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // VELOCITY Y
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // MASS
        grid.RowsProportions.Add(new Proportion(ProportionType.Auto)); // DISPLAY RADIUS

        // BODY NAME
        var bodyNameLabel = new Label
        {
            Text = "Body Name: ",
        };
        
        grid.Widgets.Add(bodyNameLabel);
        Grid.SetRow(bodyNameLabel, 0);

        var bodyNameTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "Default name"
        };
        
        grid.Widgets.Add(bodyNameTextbox);
        Grid.SetColumn(bodyNameTextbox, 1);
        
        // VEL X
        var bodyVelXLabel = new Label
        {
            Text = "Body Vel X: ",
        };
        
        grid.Widgets.Add(bodyVelXLabel);
        Grid.SetRow(bodyVelXLabel, 1);

        var bodyVelXTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "0.0",
        };
        
        grid.Widgets.Add(bodyVelXTextbox);
        Grid.SetColumn(bodyVelXTextbox, 1);
        Grid.SetRow(bodyVelXTextbox, 1);
        
        // VEL Y
        var bodyVelYLabel = new Label
        {
            Text = "Body Vel Y: ",
        };
        
        grid.Widgets.Add(bodyVelYLabel);
        Grid.SetRow(bodyVelYLabel, 2);

        var bodyVelYTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "4.0",
        };
        
        grid.Widgets.Add(bodyVelYTextbox);
        Grid.SetColumn(bodyVelYTextbox, 1);
        Grid.SetRow(bodyVelYTextbox, 2);
        
        // MASS
        var bodyMassLabel = new Label
        {
            Text = "Body Mass: ",
        };
        
        grid.Widgets.Add(bodyMassLabel);
        Grid.SetRow(bodyMassLabel, 3);

        var bodyMassTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "1e6",
        };
        
        grid.Widgets.Add(bodyMassTextbox);
        Grid.SetColumn(bodyMassTextbox, 1);
        Grid.SetRow(bodyMassTextbox, 3);
        
        // DISPLAY RADIUS
        var bodyDisplaySizeLabel = new Label
        {
            Text = "Body Size: ",
        };
        
        grid.Widgets.Add(bodyDisplaySizeLabel);
        Grid.SetRow(bodyDisplaySizeLabel, 4);

        var bodyDisplaySizeTextbox = new TextBox
        {
            MinWidth = UIConstants.DefaultTextboxWidth,
            Text = "0.05",
        };
        
        grid.Widgets.Add(bodyDisplaySizeTextbox);
        Grid.SetColumn(bodyDisplaySizeTextbox, 1);
        Grid.SetRow(bodyDisplaySizeTextbox, 4);
        
        // DIALOG AND SETUP
        var editBodyDialog = new Dialog
        {
            Title = "Edit Body",
            Content = grid
        };

        editBodyDialog.ButtonOk.Click += (sender, e) =>
        {
            Console.WriteLine("This body will be edited!");
        };

        return editBodyDialog;
    }

    private VerticalStackPanel EditPanel(SimulationData simData)
    {
        var createBodyDialogue = CreateBodyDialog(simData);
        
        var createBodyButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Create Body",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        createBodyButton.Click += (s, e) =>
        {
            createBodyDialogue.Show(_desktop);
        };
        
        var editBodyDialog = EditBodyDialog(simData);

        var editModeButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Edit Mode",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        editModeButton.Click += (sender, args) =>
        {
            simData.IsPaused = !simData.IsPaused;
            simData.EditMode = !simData.EditMode;
        };
        
        var editBodyButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Edit Body",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        editBodyButton.Click += (sender, args) =>
        {
            if (simData.EditMode && simData.IsABodySelected)
            {
                editBodyDialog.Show(_desktop);
            }
        };

        var deleteBodyButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight,
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Delete Body",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };

        deleteBodyButton.Click += (sender, args) =>
        {
            if (simData.EditMode && simData.IsABodySelected)
            {
                simData.DeleteSelectedBody = true;
            }
        };
        
        var editPanel = new VerticalStackPanel
        {
            Spacing = 8,
            Margin = new Thickness(0, 0, UIConstants.DefaultMargin, UIConstants.DefaultMargin),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
        };
        
        editPanel.Widgets.Add(deleteBodyButton);
        editPanel.Widgets.Add(editBodyButton);
        editPanel.Widgets.Add(editModeButton);
        editPanel.Widgets.Add(createBodyButton);

        return editPanel;
    }

    private Button ReturnButton()
    {
        Button returnButton = new Button
        {
            Width = UIConstants.DefaultButtonWidth,
            Height = UIConstants.DefaultButtonHeight + UIConstants.DefaultMargin,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Margin = new Thickness(0, UIConstants.DefaultMargin, UIConstants.DefaultMargin, 0),
            Content = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Return to Sim Menu",
                Font = _fontManager.LightFont(UIConstants.DefaultMargin)
            }
        };
        
        returnButton.Click += (s, e) =>
        {
            _game.GameStateManager.ChangeState(new SimulationMenu(_game));
        };
        
        return returnButton;
    }

    public void Draw()
    {
        _desktop.Render();
    }
}