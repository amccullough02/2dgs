using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace _2dgs;

public class Simulation : GameState
{
    private Desktop _desktop;
    private List<Body> _bodies;
    private SaveSystem _saveSystem;
    private SaveData _saveData;
    private SimulationData _simData;
    private SimulationUi _simUi;
    private TextureManager _textureManager;
    private FontManager _fontManager;
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
        _textureManager = new TextureManager();
        _textureManager.LoadContent(game.Content, game.GraphicsDevice);
        _fontManager = new FontManager();
        _simData = new SimulationData();
        _simUi = new SimulationUi(game, _simData);
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
    }

    private void CreateBody()
    {
        if (_simData.ToggleBodyGhost)
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                var body = new Body(
                    _simData.CreateBodyData.Name,
                    _ghostBody.Position, 
                    _simData.CreateBodyData.Velocity, 
                    _simData.CreateBodyData.Mass, 
                    _simData.CreateBodyData.DisplayRadius, 
                    _textureManager);
                
                _bodies.Add(body);
                _simData.ToggleBodyGhost = !_simData.ToggleBodyGhost;
            }
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        _mouseState = Mouse.GetState();
        _ghostBody.Update(_simData);
        CreateBody();
        
        if (!_simData.IsPaused)
        {
            foreach (Body body in _bodies)
            {
                body.Update(_bodies, _simData.TimeStep);
            }
        }

        if (_simData.EditMode)
        {
            foreach (Body body in _bodies)
            {
                body.CheckIfSelected(_mouseState.Position, _mouseState);
            }
        }
    }

    private void EditModeDisplay(SpriteBatch spriteBatch)
    {
        if (_simData.EditMode)
        {
            _fontManager.MediumFont(24)
                .DrawText(spriteBatch, "Edit Mode Active", new Vector2(10, 40), Color.Green);
        }
    } 

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        EditModeDisplay(spriteBatch);
        
        foreach (Body body in _bodies)
        {
            body.Draw(spriteBatch, _simData);
        }

        _ghostBody.Draw(spriteBatch, _textureManager, _simData);
        spriteBatch.End();
        _simUi.Draw();
    }
}