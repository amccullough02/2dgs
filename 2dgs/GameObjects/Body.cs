﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace _2dgs;

public class Body
{
    private Vector2 _position;
    public bool Selected;
    private Vector2 _velocity;
    private string _name;
    private List<Vector2> _orbit_trail;
    private float _mass;
    private int _maxTrailLength = 2000;
    private float _displayRadius;
    private const int FontSize = 20;
    private TextureManager _textureManager;
    private Color _color = Color.White;

    public Body(string name, Vector2 position, Vector2 velocity, float mass, float displayRadius, TextureManager textureManager)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displayRadius = displayRadius;
        _orbit_trail = new List<Vector2>();
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
            DisplayRadius = _displayRadius
        };
    }

    public void Edit(String name, Vector2 position, Vector2 velocity, float mass, float displayRadius)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displayRadius = displayRadius;
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
        
        if (mouseState.RightButton == ButtonState.Pressed && bodyBounds.Contains(mousePositionF)) Selected = false; 
    }

    public void Update(List<Body> bodies, int timestep)
    {
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
        
        _velocity += totalForce / _mass * timestep;
        _position += _velocity * timestep;
        _orbit_trail.Add(_position);

        if (_orbit_trail.Count >= _maxTrailLength)
        {
            _orbit_trail.RemoveAt(0);
        }
    }

    private void DrawOrbit(SpriteBatch spriteBatch, SimulationData simData, Color color, float thickness)
    {
        if (_orbit_trail.Count > 1 && simData.ToggleTrails)
        {

            int trailLength = Math.Min(simData.TrailLength, _orbit_trail.Count);
            for (int i = _orbit_trail.Count - trailLength; i < _orbit_trail.Count - 1; i++)
            {
                Vector2 direction = _orbit_trail[i] - _orbit_trail[i + 1];
                float length = direction.Length();
                float angle = (float)Math.Atan2(direction.Y, direction.X);

                spriteBatch.Draw(_textureManager.OrbitTexture,
                    _orbit_trail[i + 1],
                    null,
                    color,
                    angle,
                    Vector2.Zero,
                    new Vector2(length,
                        thickness),
                    SpriteEffects.None,
                    0f);
            }
        }
    }

    private void DrawSelector(SpriteBatch spriteBatch, SimulationData simData)
    {
        if (Selected && simData.EditMode)
        {
            spriteBatch.Draw(_textureManager.SelectorTexture,
                _position,
                null,
                _color,
                0f,
                new Vector2(_textureManager.SelectorTexture.Width / 2, _textureManager.SelectorTexture.Height / 2),
                new Vector2(_displayRadius, _displayRadius),
                SpriteEffects.None,
                0f);
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
        switch (simData.Position)
        {
            case Position.Left:
                FontManager.LightFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((_displayRadius * 600) + 5,
                            -10f),
                        Color.White);
                break;
            case Position.Right:
                FontManager.LightFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2((-_displayRadius * 600) - 5 - (FontSize * _name.Length / 1.5f),
                            -10f),
                        Color.White);
                break;
            case Position.Bottom:
                FontManager.LightFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-FontSize * _name.Length / 3, _displayRadius * 600),
                        Color.White);
                break;
            case Position.Top:
                FontManager.LightFont(FontSize)
                    .DrawText(spriteBatch,
                        _name,
                        _position +
                        new Vector2(-FontSize * _name.Length / 3, -(_displayRadius * 600) - FontSize),
                        Color.White);
                break;
            default:
                FontManager.LightFont(FontSize)
                    .DrawText(spriteBatch,
                        "Test Name",
                        _position +
                        new Vector2(_displayRadius * 600,
                            -10f),
                        Color.White);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SimulationData simData)
    {
        DrawOrbit(spriteBatch, simData, Color.White, 2f);
        DrawBody(spriteBatch);
        DrawSelector(spriteBatch, simData);
        DrawNames(simData, spriteBatch);
    }
}