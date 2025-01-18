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

public class Body
{
    public bool Selected;
    private string _name;
    private float _displayRadius;
    private float _mass;
    private const float FadeValue = 0.4f;
    private const int MaxTrailLength = 2000;
    private const int FontSize = 24;
    private Color _color;
    private readonly List<Vector2> _orbitTrail;
    private readonly TextureManager _textureManager;
    private Vector2 _velocity;
    private Vector2 _position;

    public Body(string name,
        Vector2 position,
        Vector2 velocity,
        float mass,
        float displayRadius,
        Color color,
        TextureManager textureManager)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displayRadius = displayRadius;
        _orbitTrail = [];
        _color = color;
        _textureManager = textureManager;
    }

    private Vector2 CalculateGravity(Body otherBody)
    {
        Vector2 componentDistance = otherBody._position - _position;
        float distance = componentDistance.Length();
        double forceOfGravity = 6.6743e-11 * _mass * otherBody._mass / distance * distance;
        Vector2 unitVector = componentDistance / distance;
        Vector2 forceVector = unitVector * (float)forceOfGravity;
        
        return forceVector;
    }

    public BodyData ConvertToBodyData()
    {
        return new BodyData
        {
            Name = _name,
            Position = _position,
            Velocity = _velocity,
            Mass = _mass,
            DisplayRadius = _displayRadius,
            Color = _color,
        };
    }

    public void Edit(string name, Vector2 position, Vector2 velocity, float mass, float displayRadius)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displayRadius = displayRadius;
    }

    public void ChangeColor(Color color)
    {
        _color = color;
    }

    public void CheckIfSelected(Point mousePosition, MouseState mouseState)
    {
        float trueDisplayRadius = _displayRadius * _textureManager.BodyTexture.Width;
        RectangleF bodyBounds = new RectangleF(
            _position.X - trueDisplayRadius / 2,
            _position.Y - trueDisplayRadius / 2,
            trueDisplayRadius,
            trueDisplayRadius);
        
        PointF mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        
        if (mouseState.LeftButton == ButtonState.Pressed && bodyBounds.Contains(mousePositionF)) Selected = true;
    }

    public void CheckIfDeselected(Point mousePosition, MouseState mouseState)
    {
        float trueDisplayRadius = _displayRadius * _textureManager.BodyTexture.Width;
        RectangleF bodyBounds = new RectangleF(
            _position.X - trueDisplayRadius / 2,
            _position.Y - trueDisplayRadius / 2,
            trueDisplayRadius,
            trueDisplayRadius);
        
        PointF mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        
        if (mouseState.RightButton == ButtonState.Pressed && !bodyBounds.Contains(mousePositionF)) Selected = false; 
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
            
            Vector2 force = CalculateGravity(body);
            totalForce += force;
        }
        
        _velocity += totalForce / _mass * timeStep;
        _position += _velocity * timeStep;
        _orbitTrail.Add(_position);

        if (_orbitTrail.Count >= MaxTrailLength)
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

                spriteBatch.Draw(_textureManager.OrbitTexture,
                    _orbitTrail[i + 1],
                    null,
                    _color * FadeValue,
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
            float trueDisplayRadius = _displayRadius * _textureManager.BodyTexture.Width / 2;
            float selectorOffset = trueDisplayRadius / 5;
            const float miniMumOffset = 8.0f;
            
            if (selectorOffset < miniMumOffset) selectorOffset = miniMumOffset;
            
            float radius = trueDisplayRadius + selectorOffset;
            
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
                float glowRadius = 1.0f + (i * 0.02f);
            
                spriteBatch.Draw(_textureManager.BodyTexture,
                    _position,
                    null,
                    _color * glowOpacity,
                    0f,
                    new Vector2(_textureManager.BodyTexture.Width / 2, _textureManager.BodyTexture.Height / 2),
                    new Vector2(_displayRadius * glowRadius, _displayRadius * glowRadius),
                    SpriteEffects.None,
                    0f);
            }   
        }
    }

    private void DrawBody(SpriteBatch spriteBatch)
    {
        
        if (_textureManager.BodyTexture == null)
        {
            throw new InvalidOperationException("Body has no texture!");
        }
        
        spriteBatch.Draw(_textureManager.BodyTexture,
            _position,
            null,
            _color,
            0f,
            new Vector2(_textureManager.BodyTexture.Width / 2, _textureManager.BodyTexture.Height / 2),
            new Vector2(_displayRadius, _displayRadius),
            SpriteEffects.None,
            0f);
    }

    private void DrawNames(SimulationData simData, SpriteBatch spriteBatch)
    {
        if (!simData.ToggleNames) return;

        Vector2 textSize = FontManager.MediumFont(FontSize).MeasureString(_name);
        float padding = 10f;
        
        switch (simData.Position)
        {
            case Position.Right:
                FontManager.MediumFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((_displayRadius * 600) + padding, -textSize.Y / 2),
                        _color);
                break;
            case Position.Left:
                FontManager.MediumFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((-_displayRadius * 600) - padding - textSize.X, -textSize.Y / 2),
                        _color);
                break;
            case Position.Bottom:
                FontManager.MediumFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, (_displayRadius * 600) + padding),
                        _color);
                break;
            case Position.Top:
                FontManager.MediumFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-textSize.X / 2, -(_displayRadius * 600) - padding - textSize.Y),
                        _color);
                break;
            default:
                FontManager.LightFont(FontSize)
                    .DrawText(spriteBatch,
                        "Test Name",
                        _position +
                        new Vector2(_displayRadius * 600, -10f),
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