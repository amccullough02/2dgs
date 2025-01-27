using System;
using System.Collections.Generic;
using System.Drawing;
using Apos.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

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
    private Vector2 _velocity = velocity;
    private Vector2 _position = position;
    private const float DefaultFadeValue = 0.4f;
    private const int DefaultTrailLength = 2000;
    private const int DefaultFontSize = 24;

    private Vector2 CalculateGravity(Body otherBody)
    {
        Vector2 componentDistance = otherBody._position - _position;
        float distance = componentDistance.Length();
        double forceOfGravity = 6.6743e-11 * _mass * otherBody._mass / distance * distance;
        Vector2 unitVector = componentDistance / distance;
        Vector2 forceVector = unitVector * (float)forceOfGravity;
        
        return forceVector;
    }

    public void OffsetPosition(SimulationData simulationData)
    {
        _position += simulationData.ScreenDimensions / 2;
    }

    public BodyData ConvertToBodyData(SimulationData simulationData)
    {
        return new BodyData
        {
            Name = _name,
            Position = _position - simulationData.ScreenDimensions / 2,
            Velocity = _velocity,
            Mass = _mass,
            DisplaySize = _displaySize,
            Color = _color,
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

    public void ChangeColor(Color color)
    {
        _color = color;
    }

    public void CheckIfSelected(Point mousePosition, MouseState mouseState)
    {
        float trueDisplaySize = _displaySize * textureManager.BodyTexture.Width;
        
        RectangleF bodyBounds = new RectangleF(
            _position.X - trueDisplaySize / 2,
            _position.Y - trueDisplaySize / 2,
            trueDisplaySize,
            trueDisplaySize);
        
        PointF mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        
        if (mouseState.LeftButton == ButtonState.Pressed && bodyBounds.Contains(mousePositionF)) Selected = true;
    }

    public void CheckIfDeselected(Point mousePosition, MouseState mouseState)
    {
        float trueDisplaySize = _displaySize * textureManager.BodyTexture.Width;
        
        RectangleF bodyBounds = new RectangleF(
            _position.X - trueDisplaySize / 2,
            _position.Y - trueDisplaySize / 2,
            trueDisplaySize,
            trueDisplaySize);
        
        PointF mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        
        if (mouseState.RightButton == ButtonState.Pressed && !bodyBounds.Contains(mousePositionF)) Selected = false; 
    }

    private void CheckForCollisions(Body thisBody, Body otherBody)
    {
        if (thisBody.Destroyed || otherBody.Destroyed) return;
        
        float bodySize = thisBody._displaySize * textureManager.BodyTexture.Width;

        RectangleF bodyBounds = new RectangleF
        {
            X = thisBody._position.X - bodySize / 2,
            Y = thisBody._position.Y - bodySize / 2,
            Width = bodySize,
            Height = bodySize,
        };
        
        float otherBodySize = otherBody._displaySize * textureManager.BodyTexture.Width;

        RectangleF otherBodyBounds = new RectangleF
        {
            X = otherBody._position.X - otherBodySize / 2,
            Y = otherBody._position.Y - otherBodySize / 2,
            Width = otherBodySize,
            Height = otherBodySize,
        };

        if (bodyBounds.Contains(otherBodyBounds))
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

    public void Update(List<Body> bodies, int userTimeStep, GameTime gameTime)
    {
        float timeStep = userTimeStep * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        Vector2 totalForce = Vector2.Zero;

        foreach (Body body in bodies)
        {
            if (this == body)
            {
                continue;
            }
            
            CheckForCollisions(this, body);
            Vector2 force = CalculateGravity(body);
            totalForce += force;
        }
        
        _velocity += totalForce / _mass * timeStep;
        _position += _velocity * timeStep;
        _orbitTrail.Add(_position);

        if (_orbitTrail.Count >= DefaultTrailLength)
        {
            _orbitTrail.RemoveAt(0);
        }
    }

    private void DrawOrbit(SpriteBatch spriteBatch, SimulationData simData, float thickness)
    {
        if (_orbitTrail.Count > 1 && simData.ToggleTrails)
        {

            int trailLength = Math.Min(simData.TrailLength, _orbitTrail.Count);
            for (int i = _orbitTrail.Count - trailLength; i < _orbitTrail.Count - 1; i++)
            {
                Vector2 direction = _orbitTrail[i] - _orbitTrail[i + 1];
                float length = direction.Length();
                float angle = (float)Math.Atan2(direction.Y, direction.X);

                spriteBatch.Draw(textureManager.OrbitTexture,
                    _orbitTrail[i + 1],
                    null,
                    _color * DefaultFadeValue,
                    angle,
                    Vector2.Zero,
                    new Vector2(length,
                        thickness),
                    SpriteEffects.None,
                    0f);
            }
        }
    }

    private void DrawSelector(SimulationData simData, ShapeBatch shapeBatch)
    {
        if (Selected && simData.EditMode)
        {
            float displayRadius = _displaySize * textureManager.BodyTexture.Width / 2;
            float selectorOffset = displayRadius / 5;
            const float miniMumOffset = 8.0f;
            
            if (selectorOffset < miniMumOffset) selectorOffset = miniMumOffset;
            
            float radius = displayRadius + selectorOffset;
            
            shapeBatch.Begin();
            shapeBatch.DrawCircle(_position, radius, Color.Transparent, Color.White, 3f);
            shapeBatch.End();
        }
    }

    private void DrawGlow(SpriteBatch spriteBatch, SimulationData simulationData)
    {
        if (simulationData.ToggleGlow)
        {
            for (int i = 0; i < 100; i++)
            {
                float glowOpacity = 0.07f - (i * 0.002f);
                float glowSize = 1.0f + (i * 0.02f);
            
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

    private void DrawNames(SimulationData simData, SpriteBatch spriteBatch)
    {
        if (!simData.ToggleNames) return;

        Vector2 textSize = FontManager.MediumFont(DefaultFontSize).MeasureString(_name);
        float padding = 10f;
        
        switch (simData.Position)
        {
            case Position.Right:
                FontManager.MediumFont(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((_displaySize * 600) + padding, -textSize.Y / 2),
                        _color);
                break;
            case Position.Left:
                FontManager.MediumFont(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((-_displaySize * 600) - padding - textSize.X, -textSize.Y / 2),
                        _color);
                break;
            case Position.Bottom:
                FontManager.MediumFont(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, (_displaySize * 600) + padding),
                        _color);
                break;
            case Position.Top:
                FontManager.MediumFont(DefaultFontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, -(_displaySize * 600) - padding - textSize.Y),
                        _color);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SimulationData simulationData, ShapeBatch shapeBatch)
    {
        DrawOrbit(spriteBatch, simulationData, 2f);
        DrawBody(spriteBatch);
        DrawGlow(spriteBatch, simulationData);
        DrawSelector(simulationData, shapeBatch);
        DrawNames(simulationData, spriteBatch);
    }
}