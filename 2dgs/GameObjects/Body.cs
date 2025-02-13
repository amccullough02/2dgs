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
using Rectangle = Microsoft.Xna.Framework.Rectangle;
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
    private const float FadeValue = 0.8f;
    private const float TrailThickness = 2f;
    private const float G = 6.6743e-6f;
    private const int OrbitCalculations = 1000;
    private const int MaximumTrailLength = 2000;
    private const int FontSize = 24;

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

    public void Edit(string name, Vector2 position, Vector2 velocity, float mass, float displaySize, List<Body> bodies)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displaySize = displaySize;
        
        CalculateFutureOrbits(bodies);
        PruneOrbits();
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
        var forceOfGravity = G * _mass * otherBody._mass / (distance * distance);
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
        if (_orbitTrail.Count >= MaximumTrailLength)
        {
            _orbitTrail.RemoveAt(0);
        }
    }

    private void CalculateFutureOrbits(List<Body> bodies)
    {
        var virtualBodies = bodies.Select(b => new
        { Position = b._position, Mass = b._mass }).ToList();

        var virtualVelocity = _velocity;
        var virtualPosition = _position;

        var thisBodyIndex = bodies.IndexOf(this);

        for (var i = 0; i < OrbitCalculations; i++)
        {
            var totalForce = Vector2.Zero;

            for (var j = 0; j < virtualBodies.Count; j++)
            {
                if (j == thisBodyIndex) continue;
                
                var body = virtualBodies[j];
                
                var componentDistance = body.Position - virtualPosition;
                var distance = componentDistance.Length();
                var forceOfGravity = G * _mass * body.Mass / (distance * distance);
                var unitVector = componentDistance / distance;
                var forceVector = unitVector * forceOfGravity;
                
                totalForce += forceVector;
            }
        
            virtualVelocity += totalForce / _mass;
            virtualPosition += virtualVelocity;
            _futureOrbit.Add(virtualPosition);   
        }
    }

    private void PruneOrbits()
    {
        if (_futureOrbit.Count > OrbitCalculations)
        {
            _futureOrbit.RemoveRange(0, OrbitCalculations);
        }
    }
    
    public void Update(List<Body> bodies, int userTimeStep, GameTime gameTime)
    {
        var timeStep = userTimeStep * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        UpdateOrbits(bodies, timeStep);
        CalculateFutureOrbits(bodies);
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
            var angle = MathF.Atan2(direction.Y, direction.X);

            var fadeValue = (float)(i - startIndex) / (trailLength - 1);      
            var alpha = FadeValue * fadeValue;
            
            spriteBatch.Draw(textureManager.BaseTexture,
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
            var angle = MathF.Atan2(direction.Y, direction.X);
            
            var fadeValue = 1.0f - (float)i / _futureOrbit.Count;
            var alpha = FadeValue * fadeValue;
            
            spriteBatch.Draw(textureManager.BaseTexture,
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

    private void DrawArrow(SpriteBatch spriteBatch, Color color, float length, int width, float rotation)
    {
        Vector2 RotateVector(Vector2 vector, float angle)
        {
            var cos = MathF.Cos(angle);
            var sin = MathF.Sin(angle);
            
            return new Vector2(vector.X * cos - vector.Y * sin, vector.X * sin + vector.Y * cos);
        }
        
        var trueDisplaySize = _displaySize * textureManager.BodyTexture.Width;
        var bodyCenter = new Vector2(_position.X - _displaySize / 2, _position.Y - _displaySize / 2);
        
        var arrowStemLength = (Math.Abs(length) + trueDisplaySize / 2);
        var arrowStem = new Rectangle((int)bodyCenter.X, (int)bodyCenter.Y, width, (int)arrowStemLength);
        var arrowStemRotation = rotation;
        
        var arrowTipSize = new Vector2(textureManager.ArrowTip.Width + 10, textureManager.ArrowTip.Height);
        var arrowTipLocation = bodyCenter + RotateVector(new Vector2(0, arrowStemLength), rotation);
        var arrowTipRotation = rotation + MathF.PI;

        if (length < 0)
        {
            arrowStemRotation = rotation + MathF.PI;
            arrowTipLocation = bodyCenter + RotateVector(new Vector2(0, arrowStemLength), arrowStemRotation);
            arrowTipRotation = rotation;
        }
        
        spriteBatch.Draw(textureManager.BaseTexture, arrowStem, null, color, arrowStemRotation, Vector2.Zero, SpriteEffects.None,
            0f);
        spriteBatch.Draw(textureManager.ArrowTip, arrowTipLocation, null, color, arrowTipRotation, arrowTipSize / 2,
            new Vector2(0.3f), SpriteEffects.None, 0f);
    }

    private void DrawVelocityVectors(SimulationSceneData simulationSceneData, SpriteBatch spriteBatch)
    {
        if (!simulationSceneData.ToggleVectors) return;
        
        var tangentVelocityAngle = MathF.Atan2(_velocity.Y, _velocity.X) - MathF.PI / 2;
        DrawArrow(spriteBatch, Color.White, 100, 2, tangentVelocityAngle);
        
        var xComponentVelocityAngle = MathF.PI * 1.5f;
        var xComponentArrowLength = _velocity.X * 15;
        DrawArrow(spriteBatch, Color.Red, xComponentArrowLength, 2, xComponentVelocityAngle);

        var yComponentVelocityAngle = 0f;
        var yComponentArrowLength = _velocity.Y * 15;
        DrawArrow(spriteBatch, Color.Green, yComponentArrowLength, 2, yComponentVelocityAngle);
    }

    private void DrawSelector(SimulationSceneData simSceneData, ShapeBatch shapeBatch)
    {
        if (!Selected || !simSceneData.EditMode) return;
        
        var displayRadius = _displaySize * textureManager.BodyTexture.Width / 2;
        var selectorOffset = displayRadius / 5;
        const float miniMumOffset = 8.0f;
            
        if (selectorOffset < miniMumOffset) selectorOffset = miniMumOffset;
            
        var radius = displayRadius + selectorOffset;
            
        shapeBatch.Begin();
        shapeBatch.DrawCircle(_position, radius, Color.Transparent, Color.White, 3f);
        shapeBatch.End();
    }

    private void DrawGlow(SpriteBatch spriteBatch, SimulationSceneData simulationSceneData)
    {
        if (!simulationSceneData.ToggleGlow) return;
        
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

        var textSize = FontManager.MediumText(FontSize).MeasureString(_name);
        var padding = 10f;
        
        switch (simSceneData.Position)
        {
            case Position.Right:
                FontManager.MediumText(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((_displaySize * 600) + padding, -textSize.Y / 2),
                        _color);
                break;
            case Position.Left:
                FontManager.MediumText(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((-_displaySize * 600) - padding - textSize.X, -textSize.Y / 2),
                        _color);
                break;
            case Position.Bottom:
                FontManager.MediumText(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, (_displaySize * 600) + padding),
                        _color);
                break;
            case Position.Top:
                FontManager.MediumText(FontSize)
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
        DrawTrail(spriteBatch, simulationSceneData, TrailThickness);
        DrawOrbit(spriteBatch, simulationSceneData, TrailThickness);
        DrawVelocityVectors(simulationSceneData, spriteBatch);
        DrawBody(spriteBatch);
        DrawGlow(spriteBatch, simulationSceneData);
        DrawSelector(simulationSceneData, shapeBatch);
        DrawNames(simulationSceneData, spriteBatch);
    }
}