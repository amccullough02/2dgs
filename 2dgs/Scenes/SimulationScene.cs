using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class SimulationScene : Scene
{
    private List<Body> _bodies;
    private SaveSystem _saveSystem;
    private SimulationSaveData _simulationSaveData;
    private SettingsSaveData _settingsSaveData;
    private SimulationMediator _simulationMediator;
    private SimulationUi _simulationUi;
    private TextureManager _textureManager;
    private SoundEffectPlayer _soundEffectPlayer;
    private KeyboardState _keyboardState;
    private KeyboardState _previousKeyboardState;
    private Test _test;
    private GhostBody _ghostBody;
    private ShapeBatch _shapeBatch;
    private readonly Game _game;
    
    public SimulationScene(Game game, string filePath)
    {
        _game = game;
        InitializeComponents(game, filePath);
        SetupSimulation();
        SetupLesson();
        RunTests();
    }

    private void InitializeComponents(Game game, string filePath)
    {
        #region Loading Data
        _saveSystem = new SaveSystem();
        _settingsSaveData = _saveSystem.LoadSettings();
        _simulationSaveData = _saveSystem.LoadSimulation(filePath);
        _simulationMediator = new SimulationMediator
        {
            FilePath = filePath,
            TimeStep = _simulationSaveData.DefaultTimestep,
            Lesson = _simulationSaveData.IsLesson,
            SimulationTitle = _simulationSaveData.Title,
            ScreenDimensions = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
            LessonPages = _simulationSaveData.LessonPages
        };
        #endregion
        
        #region Initializing Systems
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _soundEffectPlayer = new SoundEffectPlayer(game.Content);
        _shapeBatch = new ShapeBatch(game.GraphicsDevice, game.Content);
        _simulationUi = new SimulationUi(game, _simulationMediator);
        _test = new Test();
        #endregion
        
        _bodies = [];
        _ghostBody = new GhostBody();
    }

    private void SetupSimulation()
    {
        if (_simulationSaveData == null)
        {
            throw new FileLoadException("Simulation data could not be loaded.");
        }
        
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
        
        foreach (var body in _bodies)
        {
            body.OffsetPosition(_simulationMediator);
        }
    }

    private void SetupLesson()
    {
        if (!_simulationMediator.Lesson) return;

        var restrictedWidgets = _simulationSaveData.LessonPages[0].RestrictWidgets;
        FindWidget.DisableWidgets(_simulationUi.GetRoot(), restrictedWidgets);
    }
    
    private void RunTests()
    {
        _test.TestSimulationLoading(_simulationSaveData.Bodies.Count, _bodies.Count);
    }

    private void SaveSimulation()
    {
        if (!_simulationMediator.AttemptToSaveFile) return;
        
        Console.WriteLine("DEBUG: Saving simulation to " + _simulationMediator.FilePath);
            
        var dataToSave = new SimulationSaveData
        {
            Title = _simulationSaveData.Title,
            Description = _simulationSaveData.Description,
            ThumbnailPath = _simulationSaveData.ThumbnailPath,
            DefaultTimestep = _simulationSaveData.DefaultTimestep,
            IsLesson = _simulationSaveData.IsLesson,
            LessonPages = _simulationSaveData.LessonPages
        };

        foreach (var body in _bodies)
        {
            dataToSave.Bodies.Add(body.ConvertToBodyData(_simulationMediator));
        }
            
        _saveSystem.SaveSimulation(_simulationMediator.FilePath, dataToSave);
        _simulationMediator.AttemptToSaveFile = false;
    }

    private bool IsBodySelected()
    {
        return _bodies.Any(body => body.Selected);
    }
    
    private Body SelectedBody() { return _bodies.FirstOrDefault(body => body.Selected); }

    private void CreateBody(MouseState mouseState)
    {
        if (!_simulationMediator.ToggleBodyGhost) return;
        if (mouseState.LeftButton != ButtonState.Pressed) return;
        var body = new Body(
            _simulationMediator.CreateBodyData.Name,
            _ghostBody.Position, 
            _simulationMediator.CreateBodyData.Velocity, 
            _simulationMediator.CreateBodyData.Mass, 
            _simulationMediator.CreateBodyData.DisplaySize,
            Color.White,
            _textureManager);
                
        _bodies.Add(body);
        _simulationMediator.ToggleBodyGhost = !_simulationMediator.ToggleBodyGhost;
    }

    private void EditBody()
    {
        if (_simulationMediator.EditSelectedBody)
        {
            SelectedBody()
                .Edit(_simulationMediator.EditBodyData.Name,
                    _simulationMediator.EditBodyData.Position + _simulationMediator.ScreenDimensions / 2,
                    _simulationMediator.EditBodyData.Velocity,
                    _simulationMediator.EditBodyData.Mass,
                    _simulationMediator.EditBodyData.DisplaySize,
                    _bodies);
        }
        _simulationMediator.EditSelectedBody = false;
    }

    private void ColorBody()
    {
        if (_simulationMediator.ColorSelectedBody)
        {
            SelectedBody().SetColor(_simulationMediator.NewBodyColor);
        }
        _simulationMediator.ColorSelectedBody = false;
    }

    private void DeleteBody()
    {
        var bodiesToRemove = new List<Body>();
            
        foreach (var body in _bodies)
        {
            if (_simulationMediator.DeleteSelectedBody && body.Selected)
            {
                bodiesToRemove.Add(body);
            }
        }

        foreach (var body in bodiesToRemove)
        {
            _bodies.Remove(body);
            _soundEffectPlayer.PlayCollisionSfx();
        }
        
        _simulationMediator.DeleteSelectedBody = false;
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
        _simulationMediator.SelectedBodyData = SelectedBody().ConvertToBodyData(_simulationMediator);
    }

    private void ResetSimulation(Game game)
    {
        if (_simulationMediator.ResetSimulation)
        {
            Console.WriteLine("DEBUG: Resetting simulation");
            game.SceneManager.ChangeScene(new SimulationScene(game, _simulationMediator.FilePath));
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
            _soundEffectPlayer.PlayCollisionSfx();
        }
    }
    
    private void KeyboardShortcuts()
    {
        _keyboardState = Keyboard.GetState();
        
        KeyManager.Shortcut(_settingsSaveData.PauseShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            ((Button)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "pause_button")).DoClick();
        });
        
        KeyManager.Shortcut(_settingsSaveData.SpeedUpShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            if (_simulationMediator.TimeStep < 400) _simulationMediator.TimeStep += 10;
            var timeStepLabel = (Label)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_label");
            timeStepLabel.Text = $"Time step: {_simulationMediator.TimeStep}";
            var timeStepSlider = (HorizontalSlider)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_slider");
            timeStepSlider.Value = _simulationMediator.TimeStep;
        });
        
        KeyManager.Shortcut(_settingsSaveData.SpeedDownShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            if (_simulationMediator.TimeStep > 10) _simulationMediator.TimeStep -= 10;
            var timeStepLabel = (Label)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_label");
            timeStepLabel.Text = $"Time step: {_simulationMediator.TimeStep}";
            var timeStepSlider = (HorizontalSlider)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_slider");
            timeStepSlider.Value = _simulationMediator.TimeStep;
        });
        
        KeyManager.Shortcut(_settingsSaveData.TrailsShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationMediator.ToggleTrails = !_simulationMediator.ToggleTrails;
        });
        
        KeyManager.Shortcut(_settingsSaveData.OrbitsShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationMediator.ToggleOrbits = !_simulationMediator.ToggleOrbits;
        });
        
        KeyManager.Shortcut(_settingsSaveData.VectorsShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationMediator.ToggleVectors = !_simulationMediator.ToggleVectors;
        });
        
        KeyManager.Shortcut(_settingsSaveData.NamesShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationMediator.ToggleNames = !_simulationMediator.ToggleNames;
        });
        
        KeyManager.Shortcut(_settingsSaveData.GlowShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationMediator.ToggleGlow = !_simulationMediator.ToggleGlow;
        });
        
        KeyManager.Shortcut(_settingsSaveData.EditShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            ((Button)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "edit_mode")).DoClick();
        });
        
        KeyManager.Shortcut(_settingsSaveData.ScreenshotShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            ScreenshotManager.Capture(_game.GraphicsDevice);
        });
        
        _previousKeyboardState = _keyboardState;
    }

    private void Simulate(GameTime gameTime, MouseState mouseState)
    {
        #region Should function regardless of pause state.
        _ghostBody.Update(_simulationMediator);
        ResetSimulation(_game);
        SaveSimulation();
        CreateBody(mouseState);
        #endregion
        
        if (_simulationMediator.Paused) return;
        
        foreach (var body in _bodies)
        {
            body.Update(_bodies, _simulationMediator.TimeStep, gameTime);
        }
        
        RemoveDestroyedBodies();
    }

    private void EditMode(MouseState mouseState)
    {
        _simulationMediator.ABodySelected = IsBodySelected();
        CheckIfBodiesDeselected(mouseState);
        
        if (_simulationMediator.EditMode && !IsBodySelected())
        {
            foreach (var body in _bodies)
            {
                body.CheckIfSelected(mouseState.Position, mouseState);
            }
            
            FindWidget.DisableWidgets(_simulationUi.GetRoot(), 
                ["delete_body_button", "body_color_button", "edit_body_button"]);
        }
        
        if (_simulationMediator.EditMode && IsBodySelected())
        {
            FindWidget.EnableWidgets(_simulationUi.GetRoot(), 
                ["delete_body_button", "body_color_button", "edit_body_button"]);
            
            StoreSelectedBodyData();
            ColorBody();
            EditBody();
            DeleteBody();
        }
        
        if (!_simulationMediator.EditMode)
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

    private void DrawBackground(SpriteBatch spriteBatch)
    {
        var screenWidth = _simulationMediator.ScreenDimensions.X;
        var screenHeight = _simulationMediator.ScreenDimensions.Y;
        
        spriteBatch.Begin();
        
        spriteBatch.Draw(_textureManager.SimulationBackground, _textureManager.PositionAtCenter(screenWidth, 
            screenHeight, _textureManager.SimulationBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            _textureManager.PositionAtCenter(screenWidth, screenHeight, _textureManager.Gradient), Color.White);
        
        spriteBatch.End();
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        DrawBackground(spriteBatch);
        spriteBatch.Begin();
        
        foreach (var body in _bodies)
        {
            body.Draw(spriteBatch, _shapeBatch, _simulationMediator);
        }

        _ghostBody.Draw(spriteBatch, _textureManager, _simulationMediator);
        
        spriteBatch.End();
        _simulationUi.Draw();
    }
}