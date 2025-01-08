using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Body
{
    private Texture2D _texture;
    private Texture2D _orbitTexture;
    private Vector2 _position;
    private Vector2 _velocity;
    private string _name;
    private List<Vector2> _orbit_trail;
    private int _mass;
    private int _maxTrailLength = 2000;
    private float _displayRadius;
    private const int FontSize = 20;
    private FontManager _fontManager;

    public Body(string name, Vector2 position, Vector2 velocity, int mass, float displayRadius)
    {
        _name = name;
        _position = position;
        _velocity = velocity;
        _mass = mass;
        _displayRadius = displayRadius;
        _orbit_trail = new List<Vector2>();
        _fontManager = new FontManager();
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphics)
    {
        _texture = content.Load<Texture2D>("blank_circle");
        _orbitTexture = new Texture2D(graphics, 1, 1);
        _orbitTexture.SetData([Color.White]);
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

    private void DrawOrbit(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
    {
        Vector2 direction = end - start;
        float length = direction.Length();
        float angle = (float)Math.Atan2(direction.Y, direction.X);
        
        spriteBatch.Draw(_orbitTexture,
            start,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length,
                thickness),
            SpriteEffects.None,
            0f);
    }

    public void Draw(SpriteBatch spriteBatch, SimulationData simData)
    {
        if (_orbit_trail.Count > 1 && simData.ToggleTrails)
        {

            int trailLength = Math.Min(simData.TrailLength, _orbit_trail.Count);

            for (int i = _orbit_trail.Count - trailLength; i < _orbit_trail.Count - 1; i++)
            {
                DrawOrbit(spriteBatch, _orbit_trail[i], _orbit_trail[i + 1], Color.White, 2f);
            }
        }

        if (_texture == null)
        {
            throw new InvalidOperationException("Body has no texture!");
        }

        spriteBatch.Draw(_texture,
            _position,
            null,
            Color.White,
            0f,
            new Vector2(_texture.Width / 2, _texture.Height / 2),
            new Vector2(_displayRadius, _displayRadius),
            SpriteEffects.None,
            0f);

        if (simData.ToggleNames)

            switch (simData.Position)
            {
                case Position.Left:
                    _fontManager.LightFont(FontSize)
                        .DrawText(spriteBatch,
                            _name,
                            _position +
                            new Vector2((_displayRadius * 600) + 5,
                                -10f),
                            Color.White);
                    break;
                case Position.Right:
                    _fontManager.LightFont(FontSize)
                        .DrawText(spriteBatch,
                            _name,
                            _position +
                            new Vector2((-_displayRadius * 600) - 5 - (FontSize * _name.Length / 1.5f),
                                -10f),
                            Color.White);
                    break;
                case Position.Bottom:
                    _fontManager.LightFont(FontSize)
                        .DrawText(spriteBatch,
                            _name,
                            _position +
                            new Vector2(-FontSize * _name.Length / 3, _displayRadius * 600),
                            Color.White);
                    break;
                case Position.Top:
                    _fontManager.LightFont(FontSize)
                        .DrawText(spriteBatch,
                            _name,
                            _position +
                            new Vector2(-FontSize * _name.Length / 3, -(_displayRadius * 600) - FontSize),
                            Color.White);
                    break;
                default:
                    _fontManager.LightFont(FontSize)
                        .DrawText(spriteBatch,
                            "Test Name",
                            _position +
                            new Vector2(_displayRadius * 600,
                                -10f),
                            Color.White);
                    break;
            }
    }
}