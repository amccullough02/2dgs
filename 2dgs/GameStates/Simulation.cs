using System;
using System.Collections.Generic;
using System.Linq;
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
    
    public Simulation(Game game, string filePath)
    {
        InitializeComponents(game);
        SetupSimulation(filePath);
        RunTests();
    }

    private void InitializeComponents(Game game)
    {
        #region Systems
        _saveSystem = new SaveSystem();
        _textureManager = new TextureManager(game.Content, game.GraphicsDevice);
        _simulationData = new SimulationData();
        _simulationUi = new SimulationUi(game, _simulationData);
        _test = new Test();
        #endregion
        
        #region Components
        _bodies = new List<Body>();
        _ghostBody = new GhostBody();
        #endregion
    }

    private void RunTests()
    {
        _test.TestSimulationLoading(_saveData.Bodies.Count, _bodies.Count);
    }

    private void SetupSimulation(String filePath)
    {
        _saveData = _saveSystem.Load(filePath);

        if (_saveData?.Bodies != null)
        {
            foreach (var bodyData in _saveData.Bodies)
            {
                _bodies.Add(new Body(bodyData.Name,
                    bodyData.Position,
                    bodyData.Velocity,
                    bodyData.Mass,
                    bodyData.DisplayRadius,
                    _textureManager));
            }
        }
        
        _simulationData.FilePath = filePath;
    }

    private void SaveSimulation()
    {
        SaveData dataToSave = new SaveData();
        if (_simulationData.AttemptToSaveFile)
        {
            Console.WriteLine("DEBUG: Saving simulation to " + _simulationData.FilePath);

            foreach (Body body in _bodies)
            {
                dataToSave.Bodies.Add(body.ConvertToBodyData());
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
                    _simulationData.EditBodyData.Position,
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
        _simulationData.SelectedBodyData = GetSelectedBody().ConvertToBodyData();
    }
    
    public override void Update(GameTime gameTime)
    {
        _simulationData.IsABodySelected = IsABodySelected();
        _mouseState = Mouse.GetState();
        _ghostBody.Update(_simulationData);
        
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
            body.Draw(spriteBatch, _simulationData);
        }

        _ghostBody.Draw(spriteBatch, _textureManager, _simulationData);
        spriteBatch.End();
        _simulationUi.Draw();
    }
}