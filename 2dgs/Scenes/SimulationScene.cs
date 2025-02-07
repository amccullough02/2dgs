using System;
using System.Collections.Generic;
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
    private SimulationSceneData _simulationSceneData;
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
        SetupSimulation(filePath);
        RunTests();
    }

    private void InitializeComponents(Game game, string filePath)
    {
        #region Loading Data
        _saveSystem = new SaveSystem();
        _settingsSaveData = _saveSystem.LoadSettings();
        _simulationSaveData = _saveSystem.LoadSimulation(filePath);
        _simulationSceneData = new SimulationSceneData
        {
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
        _simulationUi = new SimulationUi(game, _simulationSceneData);
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
            body.OffsetPosition(_simulationSceneData);
        }

        _simulationSceneData.FilePath = filePath;
    }

    private void SaveSimulation()
    {
        var dataToSave = new SimulationSaveData
        {
            Title = _simulationSaveData.Title,
            IsLesson = _simulationSaveData.IsLesson,
            LessonPages = _simulationSaveData.LessonPages
        };
        
        if (_simulationSceneData.AttemptToSaveFile)
        {
            Console.WriteLine("DEBUG: Saving simulation to " + _simulationSceneData.FilePath);

            foreach (var body in _bodies)
            {
                dataToSave.Bodies.Add(body.ConvertToBodyData(_simulationSceneData));
            }
            
            _saveSystem.SaveSimulation(_simulationSceneData.FilePath, dataToSave);
            _simulationSceneData.AttemptToSaveFile = false;
        }
    }

    private bool IsBodySelected()
    {
        return _bodies.Any(body => body.Selected);
    }
    
    private Body SelectedBody() { return _bodies.FirstOrDefault(body => body.Selected); }

    private void CreateBody(MouseState mouseState)
    {
        if (!_simulationSceneData.ToggleBodyGhost) return;
        if (mouseState.LeftButton != ButtonState.Pressed) return;
        var body = new Body(
            _simulationSceneData.CreateBodyData.Name,
            _ghostBody.Position, 
            _simulationSceneData.CreateBodyData.Velocity, 
            _simulationSceneData.CreateBodyData.Mass, 
            _simulationSceneData.CreateBodyData.DisplaySize,
            Color.White,
            _textureManager);
                
        _bodies.Add(body);
        _simulationSceneData.ToggleBodyGhost = !_simulationSceneData.ToggleBodyGhost;
    }

    private void EditBody()
    {
        if (_simulationSceneData.EditSelectedBody)
        {
            SelectedBody()
                .Edit(_simulationSceneData.EditBodyData.Name,
                    _simulationSceneData.EditBodyData.Position + _simulationSceneData.ScreenDimensions / 2,
                    _simulationSceneData.EditBodyData.Velocity,
                    _simulationSceneData.EditBodyData.Mass,
                    _simulationSceneData.EditBodyData.DisplaySize);
        }
        _simulationSceneData.EditSelectedBody = false;
    }

    private void ColorBody()
    {
        if (_simulationSceneData.ColorSelectedBody)
        {
            SelectedBody().SetColor(_simulationSceneData.NewBodyColor);
        }
        _simulationSceneData.ColorSelectedBody = false;
    }

    private void DeleteBody()
    {
        var bodiesToRemove = new List<Body>();
            
        foreach (var body in _bodies)
        {
            if (_simulationSceneData.DeleteSelectedBody && body.Selected)
            {
                bodiesToRemove.Add(body);
            }
        }

        foreach (var body in bodiesToRemove)
        {
            _bodies.Remove(body);
        }
        
        _simulationSceneData.DeleteSelectedBody = false;
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
        _simulationSceneData.SelectedBodyData = SelectedBody().ConvertToBodyData(_simulationSceneData);
    }

    private void ResetSimulation(Game game)
    {
        if (_simulationSceneData.ResetSimulation)
        {
            Console.WriteLine("DEBUG: Resetting simulation");
            game.SceneManager.ChangeScene(new SimulationScene(game, _simulationSceneData.FilePath));
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
            if (_simulationSceneData.TimeStep < 400) _simulationSceneData.TimeStep += 10;
            var timeStepLabel = (Label)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_label");
            timeStepLabel.Text = $"Time step: {_simulationSceneData.TimeStep}";
            var timeStepSlider = (HorizontalSlider)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_slider");
            timeStepSlider.Value = _simulationSceneData.TimeStep;
        });
        
        KeyManager.Shortcut(_settingsSaveData.SpeedDownShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            if (_simulationSceneData.TimeStep > 10) _simulationSceneData.TimeStep -= 10;
            var timeStepLabel = (Label)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_label");
            timeStepLabel.Text = $"Time step: {_simulationSceneData.TimeStep}";
            var timeStepSlider = (HorizontalSlider)FindWidget.GetWidgetById(_simulationUi.GetRoot(), "speed_slider");
            timeStepSlider.Value = _simulationSceneData.TimeStep;
        });
        
        KeyManager.Shortcut(_settingsSaveData.TrailsShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationSceneData.ToggleTrails = !_simulationSceneData.ToggleTrails;
        });
        
        KeyManager.Shortcut(_settingsSaveData.NamesShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationSceneData.ToggleNames = !_simulationSceneData.ToggleNames;
        });
        
        KeyManager.Shortcut(_settingsSaveData.GlowShortcut, _keyboardState, _previousKeyboardState, () =>
        {
            _simulationSceneData.ToggleGlow = !_simulationSceneData.ToggleGlow;
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
        if (_simulationSceneData.Paused) return;
        
        _ghostBody.Update(_simulationSceneData);
        
        foreach (var body in _bodies)
        {
            body.Update(_bodies, _simulationSceneData.TimeStep, gameTime);
        }
        
        CreateBody(mouseState);
        RemoveDestroyedBodies();
        ResetSimulation(_game);
        SaveSimulation();
    }

    private void EditMode(MouseState mouseState)
    {
        _simulationSceneData.ABodySelected = IsBodySelected();
        CheckIfBodiesDeselected(mouseState);
        
        if (_simulationSceneData.EditMode && !IsBodySelected())
        {
            foreach (var body in _bodies)
            {
                body.CheckIfSelected(mouseState.Position, mouseState);
            }
            
            FindWidget.DisableWidgets(_simulationUi.GetRoot(), 
                ["delete_body_button", "body_color_button", "edit_body_button"]);
        }
        
        if (_simulationSceneData.EditMode && IsBodySelected())
        {
            FindWidget.EnableWidgets(_simulationUi.GetRoot(), 
                ["delete_body_button", "body_color_button", "edit_body_button"]);
            
            StoreSelectedBodyData();
            ColorBody();
            EditBody();
            DeleteBody();
        }
        
        if (!_simulationSceneData.EditMode)
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
            body.Draw(spriteBatch, _simulationSceneData, _shapeBatch);
        }

        _ghostBody.Draw(spriteBatch, _textureManager, _simulationSceneData);
        spriteBatch.End();
        _simulationUi.Draw();
    }
}