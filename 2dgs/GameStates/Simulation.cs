using System;
using System.Collections.Generic;
using System.Linq;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _2dgs;

public class Simulation : GameState
{
    private List<Body> _bodies;
    private SaveSystem _saveSystem;
    private SaveData _saveData;
    private SimulationData _simulationData;
    private SimulationUi _simulationUi;
    private TextureManager _textureManager;
    private MouseState _mouseState;
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
        _saveData = _saveSystem.Load(filePath);
        _simulationData = new SimulationData
        {
            IsLesson = _saveData.IsLesson,
            SimulationTitle = _saveData.Title,
            ScreenDimensions = new Vector2(game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height),
            LessonPages = _saveData.LessonPages
        };
        #endregion
        
        #region Initializing Systems
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _shapeBatch = new ShapeBatch(game.GraphicsDevice, game.Content);
        _simulationUi = new SimulationUi(game, _simulationData);
        _test = new Test();
        #endregion
        
        _bodies = new List<Body>();
        _ghostBody = new GhostBody();
    }

    private void RunTests()
    {
        _test.TestSimulationLoading(_saveData.Bodies.Count, _bodies.Count);
    }

    private void SetupSimulation(string filePath)
    {
        if (_saveData?.Bodies != null)
        {
            foreach (var bodyData in _saveData.Bodies)
            {
                _bodies.Add(new Body(bodyData.Name,
                    bodyData.Position,
                    bodyData.Velocity,
                    bodyData.Mass,
                    bodyData.DisplayRadius,
                    bodyData.Color,
                    _textureManager));
            }
        }
        
        foreach (Body body in _bodies)
        {
            body.OffsetPosition(_simulationData);
        }

        _simulationData.FilePath = filePath;
    }

    private void SaveSimulation()
    {
        SaveData dataToSave = new SaveData
        {
            Title = _saveData.Title,
            IsLesson = _saveData.IsLesson,
            LessonPages = _saveData.LessonPages
        };
        if (_simulationData.AttemptToSaveFile)
        {
            Console.WriteLine("DEBUG: Saving simulation to " + _simulationData.FilePath);

            foreach (Body body in _bodies)
            {
                dataToSave.Bodies.Add(body.ConvertToBodyData(_simulationData));
            }
            
            _saveSystem.Save(_simulationData.FilePath, dataToSave);
            _simulationData.AttemptToSaveFile = false;
        }
    }

    private bool IsABodySelected()
    {
        return _bodies.Any(body => body.Selected);
    }
    
    private Body GetSelectedBody() { return _bodies.FirstOrDefault(body => body.Selected); }

    private void CreateBody()
    {
        if (_simulationData.ToggleBodyGhost)
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                var body = new Body(
                    _simulationData.CreateBodyData.Name,
                    _ghostBody.Position, 
                    _simulationData.CreateBodyData.Velocity, 
                    _simulationData.CreateBodyData.Mass, 
                    _simulationData.CreateBodyData.DisplayRadius,
                    Color.White,
                    _textureManager);
                
                _bodies.Add(body);
                _simulationData.ToggleBodyGhost = !_simulationData.ToggleBodyGhost;
            }
        }
    }

    private void EditBody()
    {
        if (_simulationData.EditSelectedBody)
        {
            GetSelectedBody()
                .Edit(_simulationData.EditBodyData.Name,
                    _simulationData.EditBodyData.Position + _simulationData.ScreenDimensions / 2,
                    _simulationData.EditBodyData.Velocity,
                    _simulationData.EditBodyData.Mass,
                    _simulationData.EditBodyData.DisplayRadius);
        }
        _simulationData.EditSelectedBody = false;
    }

    private void ColorBody()
    {
        if (_simulationData.ColorSelectedBody)
        {
            GetSelectedBody().ChangeColor(_simulationData.NewBodyColor);
        }
        _simulationData.ColorSelectedBody = false;
    }

    private void DeleteBody()
    {
        var bodiesToRemove = new List<Body>();
            
        foreach (Body body in _bodies)
        {
            if (_simulationData.DeleteSelectedBody && body.Selected)
            {
                bodiesToRemove.Add(body);
            }
        }

        foreach (Body body in bodiesToRemove)
        {
            _bodies.Remove(body);
        }
        
        _simulationData.DeleteSelectedBody = false;
    }
    
    private void ForgetSelections()
    {
        foreach (Body body in _bodies) { body.Selected = false; }
    }
    
    private void CheckForDeselections()
    {
        foreach (Body body in _bodies)
        {
            body.CheckIfDeselected(_mouseState.Position, _mouseState);
        }
    }

    private void StoreSelectedBodyData()
    {
        _simulationData.SelectedBodyData = GetSelectedBody().ConvertToBodyData(_simulationData);
    }

    private void ResetSimulation(Game game)
    {
        if (_simulationData.ResetSimulation)
        {
            Console.WriteLine("DEBUG: Resetting simulation");
            game.GameStateManager.ChangeState(new Simulation(game, _simulationData.FilePath));
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        _simulationData.IsABodySelected = IsABodySelected();
        _mouseState = Mouse.GetState();
        _ghostBody.Update(_simulationData);
     
        ResetSimulation(_game);
        CheckForDeselections();
        CreateBody();
        SaveSimulation();

        if (!_simulationData.EditMode)
        {
            ForgetSelections();
        }
        
        if (!_simulationData.IsPaused)
        {
            foreach (Body body in _bodies)
            {
                body.Update(_bodies, _simulationData.TimeStep, gameTime);
            }
        }

        if (_simulationData.EditMode && !IsABodySelected())
        {
            foreach (Body body in _bodies)
            {
                body.CheckIfSelected(_mouseState.Position, _mouseState);
            }
        }

        if (_simulationData.EditMode && IsABodySelected())
        {
            StoreSelectedBodyData();
            DeleteBody();
            ColorBody();
            EditBody();
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        
        foreach (Body body in _bodies)
        {
            body.Draw(spriteBatch, _simulationData, _shapeBatch);
        }

        _ghostBody.Draw(spriteBatch, _textureManager, _simulationData);
        spriteBatch.End();
        _simulationUi.Draw();
    }
}