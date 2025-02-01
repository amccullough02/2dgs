using System;
using System.Collections.Generic;
using System.Linq;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class Simulation : GameState
{
    private List<Body> _bodies;
    private SaveSystem _saveSystem;
    private SimulationSaveData _simulationSaveData;
    private SimulationData _simulationData;
    private SimulationUi _simulationUi;
    private TextureManager _textureManager;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private Test _test;
    private GhostBody _ghostBody;
    private ShapeBatch _shapeBatch;
    private readonly Game _game;
    
    public Simulation(Game game, string filePath)
    {
        _game = game;
        InitializeComponents(game, filePath);
        SetupSimulation(filePath);
        RunTests();
    }

    private void InitializeComponents(Game game, string filePath)
    {
        #region Loading Data
        _saveSystem = new SaveSystem();
        _simulationSaveData = _saveSystem.LoadSimulation(filePath);
        _simulationData = new SimulationData
        {
            Lesson = _simulationSaveData.IsLesson,
            SimulationTitle = _simulationSaveData.Title,
            ScreenDimensions = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
            LessonPages = _simulationSaveData.LessonPages
        };
        #endregion
        
        #region Initializing Systems
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _shapeBatch = new ShapeBatch(game.GraphicsDevice, game.Content);
        _simulationUi = new SimulationUi(game, _simulationData);
        _test = new Test();
        #endregion
        
        _bodies = [];
        _ghostBody = new GhostBody();
    }

    private void RunTests()
    {
        _test.TestSimulationLoading(_simulationSaveData.Bodies.Count, _bodies.Count);
    }

    private void SetupSimulation(string filePath)
    {
        if (_simulationSaveData?.Bodies != null)
        {
            foreach (var bodyData in _simulationSaveData.Bodies)
            {
                _bodies.Add(new Body(bodyData.Name,
                    bodyData.Position,
                    bodyData.Velocity,
                    bodyData.Mass,
                    bodyData.DisplaySize,
                    bodyData.Color,
                    _textureManager));
            }
        }
        
        foreach (var body in _bodies)
        {
            body.OffsetPosition(_simulationData);
        }

        _simulationData.FilePath = filePath;
    }

    private void SaveSimulation()
    {
        var dataToSave = new SimulationSaveData
        {
            Title = _simulationSaveData.Title,
            IsLesson = _simulationSaveData.IsLesson,
            LessonPages = _simulationSaveData.LessonPages
        };
        
        if (_simulationData.AttemptToSaveFile)
        {
            Console.WriteLine("DEBUG: Saving simulation to " + _simulationData.FilePath);

            foreach (var body in _bodies)
            {
                dataToSave.Bodies.Add(body.ConvertToBodyData(_simulationData));
            }
            
            _saveSystem.SaveSimulation(_simulationData.FilePath, dataToSave);
            _simulationData.AttemptToSaveFile = false;
        }
    }

    private bool IsBodySelected()
    {
        return _bodies.Any(body => body.Selected);
    }
    
    private Body SelectedBody() { return _bodies.FirstOrDefault(body => body.Selected); }

    private void CreateBody(MouseState mouseState)
    {
        if (!_simulationData.ToggleBodyGhost) return;
        if (mouseState.LeftButton != ButtonState.Pressed) return;
        var body = new Body(
            _simulationData.CreateBodyData.Name,
            _ghostBody.Position, 
            _simulationData.CreateBodyData.Velocity, 
            _simulationData.CreateBodyData.Mass, 
            _simulationData.CreateBodyData.DisplaySize,
            Color.White,
            _textureManager);
                
        _bodies.Add(body);
        _simulationData.ToggleBodyGhost = !_simulationData.ToggleBodyGhost;
    }

    private void EditBody()
    {
        if (_simulationData.EditSelectedBody)
        {
            SelectedBody()
                .Edit(_simulationData.EditBodyData.Name,
                    _simulationData.EditBodyData.Position + _simulationData.ScreenDimensions / 2,
                    _simulationData.EditBodyData.Velocity,
                    _simulationData.EditBodyData.Mass,
                    _simulationData.EditBodyData.DisplaySize);
        }
        _simulationData.EditSelectedBody = false;
    }

    private void ColorBody()
    {
        if (_simulationData.ColorSelectedBody)
        {
            SelectedBody().SetColor(_simulationData.NewBodyColor);
        }
        _simulationData.ColorSelectedBody = false;
    }

    private void DeleteBody()
    {
        var bodiesToRemove = new List<Body>();
            
        foreach (var body in _bodies)
        {
            if (_simulationData.DeleteSelectedBody && body.Selected)
            {
                bodiesToRemove.Add(body);
            }
        }

        foreach (var body in bodiesToRemove)
        {
            _bodies.Remove(body);
        }
        
        _simulationData.DeleteSelectedBody = false;
    }
    
    private void ForgetSelections()
    {
        foreach (var body in _bodies) { body.Selected = false; }
    }
    
    private void CheckIfBodiesDeselected(MouseState mouseState)
    {
        foreach (var body in _bodies)
        {
            body.CheckIfDeselected(mouseState.Position, mouseState);
        }
    }

    private void StoreSelectedBodyData()
    {
        _simulationData.SelectedBodyData = SelectedBody().ConvertToBodyData(_simulationData);
    }

    private void ResetSimulation(Game game)
    {
        if (_simulationData.ResetSimulation)
        {
            Console.WriteLine("DEBUG: Resetting simulation");
            game.GameStateManager.ChangeState(new Simulation(game, _simulationData.FilePath));
        }
    }

    private void RemoveDestroyedBodies()
    {
        var destroyedBodies = new List<Body>();
            
        foreach (var body in _bodies)
        {
            if (body.Destroyed) destroyedBodies.Add(body);
        }

        foreach (var body in destroyedBodies)
        {
            _bodies.Remove(body);
        }
    }
    
    private void KeyboardShortcuts()
    {
        _keyboardState = Keyboard.GetState();
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.P], _keyboardState, _previousKeyboardState, () =>
        {
            ((Button)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "pause_button")).DoClick();
        });
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.Right], _keyboardState, _previousKeyboardState, () =>
        {
            if (_simulationData.TimeStep < 400) _simulationData.TimeStep += 10;
            var timeStepLabel = (Label)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_label");
            timeStepLabel.Text = $"Time step: {_simulationData.TimeStep}";
            var timeStepSlider = (HorizontalSlider)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_slider");
            timeStepSlider.Value = _simulationData.TimeStep;
        });
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.Left], _keyboardState, _previousKeyboardState, () =>
        {
            if (_simulationData.TimeStep > 10) _simulationData.TimeStep -= 10;
            var timeStepLabel = (Label)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_label");
            timeStepLabel.Text = $"Time step: {_simulationData.TimeStep}";
            var timeStepSlider = (HorizontalSlider)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_slider");
            timeStepSlider.Value = _simulationData.TimeStep;
        });
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.T], _keyboardState, _previousKeyboardState, () =>
        {
            _simulationData.ToggleTrails = !_simulationData.ToggleTrails;
        });
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.N], _keyboardState, _previousKeyboardState, () =>
        {
            _simulationData.ToggleNames = !_simulationData.ToggleNames;
        });
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.G], _keyboardState, _previousKeyboardState, () =>
        {
            _simulationData.ToggleGlow = !_simulationData.ToggleGlow;
        });
        
        KeyManager.Shortcut([Keys.LeftControl, Keys.E], _keyboardState, _previousKeyboardState, () =>
        {
            ((Button)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "edit_mode")).DoClick();
        });
        
        KeyManager.Shortcut([Keys.F11], _keyboardState, _previousKeyboardState, () =>
        {
            ScreenshotManager.Capture(_game.GraphicsDevice);
        });
        
        _previousKeyboardState = _keyboardState;
    }

    private void Simulate(GameTime gameTime, MouseState mouseState)
    {
        if (_simulationData.Paused) return;
        
        _ghostBody.Update(_simulationData);
        
        foreach (var body in _bodies)
        {
            body.Update(_bodies, _simulationData.TimeStep, gameTime);
        }
        
        CreateBody(mouseState);
        RemoveDestroyedBodies();
        ResetSimulation(_game);
        SaveSimulation();
    }

    private void EditMode(MouseState mouseState)
    {
        _simulationData.ABodySelected = IsBodySelected();
        CheckIfBodiesDeselected(mouseState);
        
        if (_simulationData.EditMode && !IsBodySelected())
        {
            foreach (var body in _bodies)
            {
                body.CheckIfSelected(mouseState.Position, mouseState);
            }
            
            FindWidget.DisableWidgets(_simulationUi.GetRoot(), 
                ["delete_body_button", "body_color_button", "edit_body_button"]);
        }
        
        if (_simulationData.EditMode && IsBodySelected())
        {
            FindWidget.EnableWidgets(_simulationUi.GetRoot(), 
                ["delete_body_button", "body_color_button", "edit_body_button"]);
            
            StoreSelectedBodyData();
            ColorBody();
            EditBody();
            DeleteBody();
        }
        
        if (!_simulationData.EditMode)
        {
            ForgetSelections();
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        KeyboardShortcuts();
        Simulate(gameTime, mouseState);
        EditMode(mouseState);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        foreach (var body in _bodies)
        {
            body.Draw(spriteBatch, _simulationData, _shapeBatch);
        }

        _ghostBody.Draw(spriteBatch, _textureManager, _simulationData);
        spriteBatch.End();
        _simulationUi.Draw();
    }
}