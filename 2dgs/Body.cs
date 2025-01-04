using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class Body
{
    private Texture2D _texture;
    private Vector2 _position;
    private Vector2 _velocity;
    private int _mass;
    private float _displayRadius;

    public Body(Vector2 position, int mass, float displayRadius)
    {
        this._position = position;
        this._velocity = Vector2.Zero;
        this._mass = mass;
        this._displayRadius = displayRadius;
    }

    public void LoadContent(ContentManager content)
    {
        _texture = content.Load<Texture2D>("blank_circle");
    }

    private Vector2 CalculateGravity(Body otherBody)
    {
        Vector2 componentDistance = otherBody._position - this._position;
        float distance = componentDistance.Length();
        double forceOfGravity = 6.6743e-11 * this._mass * otherBody._mass / distance * distance;
        Vector2 unitVector = componentDistance / distance;
        Vector2 forceVector = unitVector * (float)forceOfGravity;
        
        return forceVector;
    }

    public void Update(List<Body> bodies)
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
        
        this._velocity += totalForce / this._mass;
        this._position += this._velocity;
    }

    public void Draw(SpriteBatch spriteBatch)
    {

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
            new Vector2(this._displayRadius, this._displayRadius),
            SpriteEffects.None,
            0f);
    }
}