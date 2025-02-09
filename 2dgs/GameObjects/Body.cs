using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace _2dgs;

public class Body(
    string name,
    Vector2 position,
    Vector2 velocity,
    float mass,
    float displaySize,
    Color color,
    TextureManager textureManager)
{
    public bool Selected;
    public bool Destroyed;
    private string _name = name;
    private float _displaySize = displaySize;
    private float _mass = mass;
    private Color _color = color;
    private readonly List<Vector2> _orbitTrail = [];
    private readonly List<Vector2> _futureOrbit = [];
    private Vector2 _velocity = velocity;
    private Vector2 _position = position;
    private const float DefaultFadeValue = 0.8f;
    private const float DefaultTrailThickness = 2f;
    private const float G = 6.6743e-11f;
    private const int FutureOrbitCalculations = 1000;
    private const int DefaultTrailLength = 2000;
    private const int DefaultFontSize = 24;

    public void OffsetPosition(SimulationSceneData simulationSceneData)
    {
        _position += simulationSceneData.ScreenDimensions / 2;
    }

    public BodyData ConvertToBodyData(SimulationSceneData simulationSceneData)
    {
        return new BodyData
        {
            Name = _name,
            Position = _position - simulationSceneData.ScreenDimensions / 2,
            Velocity = _velocity,
            Mass = _mass,
            DisplaySize = _displaySize,
            Color = _color,
        };
    }

    private RectangleF GetBoundingBox()
    {
        var trueDisplaySize = _displaySize * textureManager.BodyTexture.Width;

        return new RectangleF
        {
            X = _position.X - trueDisplaySize / 2,
            Y = _position.Y - trueDisplaySize / 2,
            Width = trueDisplaySize,
            Height = trueDisplaySize
        };
    }

    public void Edit(string name, Vector2 position, Vector2 velocity, float mass, float displaySize)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displaySize = displaySize;
    }

    public void SetColor(Color color)
    {
        _color = color;
    }

    public void CheckIfSelected(Point mousePosition, MouseState mouseState)
    {
        var mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        if (mouseState.LeftButton == ButtonState.Pressed && GetBoundingBox().Contains(mousePositionF)) Selected = true;
    }

    public void CheckIfDeselected(Point mousePosition, MouseState mouseState)
    {
        var mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        if (mouseState.RightButton == ButtonState.Pressed && !GetBoundingBox().Contains(mousePositionF)) Selected = false;
    }

    private void CheckForCollisions(Body thisBody, Body otherBody)
    {
        if (thisBody.Destroyed || otherBody.Destroyed) return;

        var bodyBounds = thisBody.GetBoundingBox();

        var otherBodyBounds = otherBody.GetBoundingBox();

        if (bodyBounds.IntersectsWith(otherBodyBounds))
        {
            HandleCollision(thisBody, otherBody);
        }
    }

    private void HandleCollision(Body thisBody, Body otherBody)
    {
        if (thisBody._mass >= otherBody._mass)
        {
            thisBody._mass += otherBody._mass;
            thisBody._displaySize += otherBody._displaySize / 10.0f;
            otherBody.Destroyed = true;
        }
        else
        {
            otherBody._mass += thisBody._mass;
            otherBody._displaySize += thisBody._displaySize / 10.0f;
            thisBody.Destroyed = true;
        }
    }

    private Vector2 CalculateGravity(Body otherBody)
    {
        var componentDistance = otherBody._position - _position;
        var distance = componentDistance.Length();
        var forceOfGravity = G * _mass * otherBody._mass / distance * distance;
        var unitVector = componentDistance / distance;
        var forceVector = unitVector * forceOfGravity;
        
        return forceVector;
    }

    private void UpdateOrbits(List<Body> bodies, float timeStep)
    {
        var totalForce = Vector2.Zero;

        foreach (var body in bodies)
        {
            if (this == body)
            {
                continue;
            }
            
            CheckForCollisions(this, body);
            var force = CalculateGravity(body);
            totalForce += force;
        }
        
        _velocity += totalForce / _mass * timeStep;
        _position += _velocity * timeStep;
        _orbitTrail.Add(_position);
    }

    private void PruneTrails()
    {
        if (_orbitTrail.Count >= DefaultTrailLength)
        {
            _orbitTrail.RemoveAt(0);
        }
    }

    private void CalculateFutureOrbits(List<Body> bodies, float timeStep)
    {
        var virtualBodies = bodies.Select(b => new
        { Position = b._position, Mass = b._mass }).ToList();

        var virtualVelocity = _velocity;
        var virtualPosition = _position;

        var thisBodyIndex = bodies.IndexOf(this);

        for (var i = 0; i < FutureOrbitCalculations; i++)
        {
            var totalForce = Vector2.Zero;

            for (var j = 0; j < virtualBodies.Count; j++)
            {
                if (j == thisBodyIndex) continue;
                
                var body = virtualBodies[j];
                
                var componentDistance = body.Position - virtualPosition;
                var distance = componentDistance.Length();
                var forceOfGravity = G * _mass * body.Mass / distance * distance;
                var unitVector = componentDistance / distance;
                var forceVector = unitVector * forceOfGravity;
                
                totalForce += forceVector;
            }
        
            virtualVelocity += totalForce / _mass * timeStep;
            virtualPosition += virtualVelocity * timeStep;
            _futureOrbit.Add(virtualPosition);   
        }
    }

    private void PruneOrbits()
    {
        if (_futureOrbit.Count > FutureOrbitCalculations)
        {
            _futureOrbit.RemoveRange(0, FutureOrbitCalculations);
        }
    }
    
    public void Update(List<Body> bodies, int userTimeStep, GameTime gameTime)
    {
        var timeStep = userTimeStep * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        UpdateOrbits(bodies, timeStep);
        CalculateFutureOrbits(bodies, timeStep);
        PruneTrails();
        PruneOrbits();
    }

    private void DrawTrail(SpriteBatch spriteBatch, SimulationSceneData simulationSceneData, float thickness)
    {
        if (_orbitTrail.Count <= 1 || !simulationSceneData.ToggleTrails) return;
        
        var trailLength = Math.Min(simulationSceneData.TrailLength, _orbitTrail.Count);
        var startIndex = _orbitTrail.Count - trailLength;
        
        for (var i = _orbitTrail.Count - trailLength; i < _orbitTrail.Count - 1; i++)
        {
            var direction = _orbitTrail[i] - _orbitTrail[i + 1];
            var length = direction.Length();
            var angle = (float)Math.Atan2(direction.Y, direction.X);

            var fadeValue = (float)(i - startIndex) / (trailLength - 1);      
            var alpha = DefaultFadeValue * fadeValue;
            
            spriteBatch.Draw(textureManager.OrbitTexture,
                _orbitTrail[i + 1],
                null,
                _color * alpha,
                angle,
                Vector2.Zero,
                new Vector2(length,
                    thickness),
                SpriteEffects.None,
                0f);
        }
    }

    private void DrawOrbit(SpriteBatch spriteBatch, SimulationSceneData simulationSceneData, float thickness)
    {
        if (_futureOrbit.Count <= 1 || !simulationSceneData.ToggleOrbits) return;
        
        for (var i = 0; i < _futureOrbit.Count - 1; i++)
        {
            var direction = _futureOrbit[i] - _futureOrbit[i + 1];
            var length = direction.Length();
            var angle = (float)Math.Atan2(direction.Y, direction.X);
            
            var fadeValue = 1.0f - (float)i / _futureOrbit.Count;
            var alpha = DefaultFadeValue * fadeValue;
            
            spriteBatch.Draw(textureManager.OrbitTexture,
                _futureOrbit[i + 1],
                null,
                _color * alpha,
                angle,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0f);
        }
    }

    private void DrawSelector(SimulationSceneData simSceneData, ShapeBatch shapeBatch)
    {
        if (Selected && simSceneData.EditMode)
        {
            var displayRadius = _displaySize * textureManager.BodyTexture.Width / 2;
            var selectorOffset = displayRadius / 5;
            const float miniMumOffset = 8.0f;
            
            if (selectorOffset < miniMumOffset) selectorOffset = miniMumOffset;
            
            var radius = displayRadius + selectorOffset;
            
            shapeBatch.Begin();
            shapeBatch.DrawCircle(_position, radius, Color.Transparent, Color.White, 3f);
            shapeBatch.End();
        }
    }

    private void DrawGlow(SpriteBatch spriteBatch, SimulationSceneData simulationSceneData)
    {
        if (simulationSceneData.ToggleGlow)
        {
            for (var i = 0; i < 100; i++)
            {
                var glowOpacity = 0.07f - (i * 0.002f);
                var glowSize = 1.0f + (i * 0.02f);
            
                spriteBatch.Draw(textureManager.BodyTexture,
                    _position,
                    null,
                    _color * glowOpacity,
                    0f,
                    new Vector2(textureManager.BodyTexture.Width / 2.0f, textureManager.BodyTexture.Height / 2.0f),
                    new Vector2(_displaySize * glowSize, _displaySize * glowSize),
                    SpriteEffects.None,
                    0f);
            }   
        }
    }

    private void DrawBody(SpriteBatch spriteBatch)
    {
        
        if (textureManager.BodyTexture == null)
        {
            throw new InvalidOperationException("Body has no texture!");
        }
        
        spriteBatch.Draw(textureManager.BodyTexture,
            _position,
            null,
            _color,
            0f,
            new Vector2(textureManager.BodyTexture.Width / 2.0f, textureManager.BodyTexture.Height / 2.0f),
            new Vector2(_displaySize, _displaySize),
            SpriteEffects.None,
            0f);
    }

    private void DrawNames(SimulationSceneData simSceneData, SpriteBatch spriteBatch)
    {
        if (!simSceneData.ToggleNames) return;

        var textSize = FontManager.MediumText(DefaultFontSize).MeasureString(_name);
        var padding = 10f;
        
        switch (simSceneData.Position)
        {
            case Position.Right:
                FontManager.MediumText(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((_displaySize * 600) + padding, -textSize.Y / 2),
                        _color);
                break;
            case Position.Left:
                FontManager.MediumText(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((-_displaySize * 600) - padding - textSize.X, -textSize.Y / 2),
                        _color);
                break;
            case Position.Bottom:
                FontManager.MediumText(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, (_displaySize * 600) + padding),
                        _color);
                break;
            case Position.Top:
                FontManager.MediumText(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, -(_displaySize * 600) - padding - textSize.Y),
                        _color);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SimulationSceneData simulationSceneData, ShapeBatch shapeBatch)
    {
        DrawTrail(spriteBatch, simulationSceneData, DefaultTrailThickness);
        DrawOrbit(spriteBatch, simulationSceneData, DefaultTrailThickness);
        DrawBody(spriteBatch);
        DrawGlow(spriteBatch, simulationSceneData);
        DrawSelector(simulationSceneData, shapeBatch);
        DrawNames(simulationSceneData, spriteBatch);
    }
}