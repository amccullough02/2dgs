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

/// <summary>
/// A scene for simulating bodies.
/// </summary>
public class SimulationScene : Scene
{
    /// <summary>
    /// The bodies being simulated.
    /// </summary>
    private List<Body> _bodies;
    /// <summary>
    /// Used to save/load the simulation.
    /// </summary>
    private SaveSystem _saveSystem;
    /// <summary>
    /// Used to store data loaded from the simulation file.
    /// </summary>
    private SimulationSaveData _simulationSaveData;
    /// <summary>
    /// Used to obtain the keyboard shortcuts for certain actions. 
    /// </summary>
    private SettingsSaveData _settingsSaveData;
    /// <summary>
    /// An instance of the SimulationMediator.
    /// </summary>
    private SimulationMediator _simulationMediator;
    /// <summary>
    /// The user interface for the Simulation scene.
    /// </summary>
    private SimulationUi _simulationUi;
    /// <summary>
    /// An instance of the 2DGS Texture Manager.
    /// </summary>
    private TextureManager _textureManager;
    /// <summary>
    /// Used to play the collision sound effects.
    /// </summary>
    private SoundEffectPlayer _soundEffectPlayer;
    /// <summary>
    /// Used to get the keyboard state.
    /// </summary>
    private KeyboardState _keyboardState;
    /// <summary>
    /// A second instance of the KeyboardState to prevent repeated actions.
    /// </summary>
    private KeyboardState _previousKeyboardState;
    /// <summary>
    /// An instance of the 'ghost body' used for adding bodies.
    /// </summary>
    private GhostBody _ghostBody;
    /// <summary>
    /// An Apos.Shapes ShapeBatch class, passed into the Body draw method.
    /// </summary>
    private ShapeBatch _shapeBatch;
    /// <summary>
    /// A reference to the MonoGame Game instance.
    /// </summary>
    private readonly Game _game;
    
    /// <summary>
    /// A constructor for the SimulationScene.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    /// <param name="filePath">The path of the simulation to be loaded and saved to.</param>
    public SimulationScene(Game game, string filePath)
    {
        _game = game;
        InitializeComponents(game, filePath);
        SetupSimulation();
        SetupLesson();
        RunTests();
    }

    /// <summary>
    /// A helper method to initialize the components and systems used within the simulation.
    /// </summary>
    /// <param name="game"></param>
    /// <param name="filePath"></param>
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
        #endregion
        
        _bodies = [];
        _ghostBody = new GhostBody();
    }

    /// <summary>
    /// A method to set up the simulation by populating the _bodies list.
    /// </summary>
    /// <exception cref="FileLoadException">An exception returned when the simulation cannot be loaded.</exception>
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

    /// <summary>
    /// Sets up the lesson part of the simulation.
    /// </summary>
    private void SetupLesson()
    {
        if (!_simulationMediator.Lesson) return;

        var restrictedWidgets = _simulationSaveData.LessonPages[0].RestrictWidgets;
        FindWidget.DisableWidgets(_simulationUi.GetRoot(), restrictedWidgets);
    }

    /// <summary>
    /// Runs generic simulation-specific tests.
    /// </summary>
    private void RunTests()
    {
        TestRunner.AssertBodiesDataIntegrity(_bodies, _simulationSaveData.Bodies);
        TestRunner.AssertLessonDataIntegrity(_simulationMediator.LessonPages, _simulationSaveData.LessonPages);
    }

    /// <summary>
    /// Saves the simulation.
    /// </summary>
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
        TestRunner.AssertSimulationSaved(_simulationMediator.FilePath, dataToSave, _saveSystem);
        _simulationMediator.AttemptToSaveFile = false;
    }

    /// <summary>
    /// A method to check if a body is selected.
    /// </summary>
    /// <returns></returns>
    private bool IsBodySelected()
    {
        return _bodies.Any(body => body.Selected);
    }
    
    /// <summary>
    /// A method to obtain the selected body.
    /// </summary>
    /// <returns></returns>
    private Body SelectedBody() { return _bodies.FirstOrDefault(body => body.Selected); }

    /// <summary>
    /// A method used to create a new body.
    /// </summary>
    /// <param name="mouseState"></param>
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
        TestRunner.AssertBodyCreated(_bodies, body);
        _simulationMediator.ToggleBodyGhost = !_simulationMediator.ToggleBodyGhost;
    }

    /// <summary>
    /// A method used to edit a body.
    /// </summary>
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
            TestRunner.AssertBodyEdited(_bodies, SelectedBody());
        }
        _simulationMediator.EditSelectedBody = false;
    }

    /// <summary>
    /// A method used to set a new color for a body.
    /// </summary>
    private void ColorBody()
    {
        if (_simulationMediator.ColorSelectedBody)
        {
            SelectedBody().SetColor(_simulationMediator.NewBodyColor);
        }
        _simulationMediator.ColorSelectedBody = false;
    }

    /// <summary>
    /// A method used to delete an existing body.
    /// </summary>
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

        if (bodiesToRemove.Count > 0)
        {
            foreach (var body in bodiesToRemove)
            {
                _bodies.Remove(body);
                _soundEffectPlayer.PlayCollisionSfx();
                TestRunner.AssertBodyDeleted(_bodies, body);
            }
        }

        _simulationMediator.DeleteSelectedBody = false;
    }
    
    /// <summary>
    /// Ensures all bodies are no longer selected (used when leaving edit mode).
    /// </summary>
    private void ForgetSelections()
    {
        foreach (var body in _bodies) { body.Selected = false; }
    }
    
    /// <summary>
    /// A method that iterates through the body list to check if they've been de-selected.
    /// </summary>
    /// <param name="mouseState"></param>
    private void CheckIfBodiesDeselected(MouseState mouseState)
    {
        foreach (var body in _bodies)
        {
            body.CheckIfDeselected(mouseState.Position, mouseState);
        }
    }

    /// <summary>
    /// Stores a copy of the select bodies' data in the SimulationMediator.
    /// </summary>
    private void StoreSelectedBodyData()
    {
        _simulationMediator.SelectedBodyData = SelectedBody().ConvertToBodyData(_simulationMediator);
    }

    /// <summary>
    /// A method used to reset the simulation.
    /// </summary>
    /// <param name="game">A reference to the MonoGame Game instance.</param>
    private void ResetSimulation(Game game)
    {
        if (_simulationMediator.ResetSimulation)
        {
            Console.WriteLine("DEBUG: Resetting simulation");
            game.SceneManager.ChangeScene(new SimulationScene(game, _simulationMediator.FilePath));
        }
    }

    /// <summary>
    /// Removes all destroyed (collided) bodies in the simulation.
    /// </summary>
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
    
    /// <summary>
    /// A method that listens for keyboard shortcuts.
    /// </summary>
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

    /// <summary>
    /// A method that updates bodies within the simulation.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    /// <param name="mouseState">A reference to a MonoGame MouseState instance.</param>
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

    /// <summary>
    /// A method used to handle 'edit mode' operations.
    /// </summary>
    /// <param name="mouseState">A reference to a MonoGame MouseState instance.</param>
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
    
    /// <summary>
    /// The SimulationScene update method.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        KeyboardShortcuts();
        Simulate(gameTime, mouseState);
        EditMode(mouseState);
    }

    /// <summary>
    /// A method that draws the simulation background (its background texture and a gradient).
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    private void DrawBackground(SpriteBatch spriteBatch)
    {
        var screenWidth = _simulationMediator.ScreenDimensions.X;
        var screenHeight = _simulationMediator.ScreenDimensions.Y;
        
        spriteBatch.Begin();
        
        spriteBatch.Draw(_textureManager.SimulationBackground, TextureManager.PositionAtCenter(screenWidth, 
            screenHeight, _textureManager.SimulationBackground), Color.White);
        spriteBatch.Draw(_textureManager.Gradient,
            TextureManager.PositionAtCenter(screenWidth, screenHeight, _textureManager.Gradient), Color.White);
        
        spriteBatch.End();
    }

    /// <summary>
    /// The SimulationScene draw method.
    /// </summary>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
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