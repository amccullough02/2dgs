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

/// <summary>
/// A class used to represent bodies in the 2DGS application.
/// </summary>
/// <param name="name">The name displayed next to the body.</param>
/// <param name="position">The position of the body.</param>
/// <param name="velocity">The velocity of the body.</param>
/// <param name="mass">The mass of the body.</param>
/// <param name="displaySize">The size of the body as displayed on-screen.</param>
/// <param name="color">The color of the body.</param>
/// <param name="textureManager">An instance of the TextureManager, used to obtain a Body texture.</param>
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

    /// <summary>
    /// Offsets the position of bodies such that they appear in the relative center of the screen.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator.</param>
    public void OffsetPosition(SimulationMediator simulationMediator)
    {
        _position += simulationMediator.ScreenDimensions / 2;
    }

    /// <summary>
    /// A helper method to convert an instance of a body to a BodyData object for serialization.
    /// </summary>
    /// <param name="simulationMediator">A reference to the SimulationMediator.</param>
    /// <returns></returns>
    public BodyData ConvertToBodyData(SimulationMediator simulationMediator)
    {
        return new BodyData
        {
            Name = _name,
            Position = _position - simulationMediator.ScreenDimensions / 2,
            Velocity = _velocity,
            Mass = _mass,
            DisplaySize = _displaySize,
            Color = _color,
        };
    }

    /// <summary>
    /// Returns the bounding box of the body (useful for handling selection and collision).
    /// </summary>
    /// <param name="marginOfError">A user defined margin of error, used to ensure collision bounds aren't too generous.</param>
    /// <returns></returns>
    private RectangleF GetBoundingBox(float marginOfError = 1.0f)
    {
        var trueDisplaySize = _displaySize * textureManager.BodyTexture.Width;

        return new RectangleF
        {
            X = _position.X - trueDisplaySize * marginOfError / 2,
            Y = _position.Y - trueDisplaySize * marginOfError / 2,
            Width = trueDisplaySize * marginOfError,
            Height = trueDisplaySize * marginOfError,
        };
    }

    /// <summary>
    /// A method used when editing a body instance.
    /// </summary>
    /// <param name="name">The new name of the body.</param>
    /// <param name="position">The new position of the body.</param>
    /// <param name="velocity">The new velocity of the body.</param>
    /// <param name="mass">The new mass of the body.</param>
    /// <param name="displaySize">The new display size of the body.</param>
    /// <param name="bodies">A reference to the bodies data structure, used for updating the projected orbits of bodies post-edit.</param>
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

    /// <summary>
    /// A method used to update a body's color.
    /// </summary>
    /// <param name="color">The new color of the body.</param>
    public void SetColor(Color color)
    {
        _color = color;
    }

    /// <summary>
    /// A method used to check if a body is selected (the mouse is within the bounds of the body).
    /// </summary>
    /// <param name="mousePosition">The current position of the mouse.</param>
    /// <param name="mouseState">The state of the mouse (which buttons are being clicked).</param>
    public void CheckIfSelected(Point mousePosition, MouseState mouseState)
    {
        var mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        if (mouseState.LeftButton == ButtonState.Pressed && GetBoundingBox().Contains(mousePositionF)) Selected = true;
    }

    /// <summary>
    /// A method used to check if a body is de-selected (the mouse is within the bounds of the body).
    /// </summary>
    /// <param name="mousePosition">The current position of the mouse.</param>
    /// <param name="mouseState">The state of the mouse (which buttons are being clicked).</param>
    public void CheckIfDeselected(Point mousePosition, MouseState mouseState)
    {
        var mousePositionF = new PointF(mousePosition.X, mousePosition.Y);
        if (mouseState.RightButton == ButtonState.Pressed && !GetBoundingBox().Contains(mousePositionF)) Selected = false;
    }

    /// <summary>
    /// A method used to check if two bodies have collided.
    /// </summary>
    /// <param name="thisBody">The current body instance.</param>
    /// <param name="otherBody">Another body in the bodies data structure.</param>
    private void CheckForCollisions(Body thisBody, Body otherBody)
    {
        if (thisBody.Destroyed || otherBody.Destroyed) return;

        const float collisionTolerance = 0.8f;

        var bodyBounds = thisBody.GetBoundingBox(collisionTolerance);

        var otherBodyBounds = otherBody.GetBoundingBox(collisionTolerance);

        if (bodyBounds.IntersectsWith(otherBodyBounds))
        {
            HandleCollision(thisBody, otherBody);
        }
    }

    /// <summary>
    /// A method used to handle the aftermath of a collision, namely marking the less massive body for deletion.
    /// </summary>
    /// <param name="thisBody">The current body instance.</param>
    /// <param name="otherBody">Another body in the bodies data structure.</param>
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

    /// <summary>
    /// A method used to calculate the gravitational interactions.
    /// </summary>
    /// <param name="otherBody">Another body in the bodies data structure.</param>
    /// <returns>A force vector representing the gravitational forces affecting the body.</returns>
    private Vector2 CalculateGravity(Body otherBody)
    {
        var componentDistance = otherBody._position - _position;
        var distance = componentDistance.Length();
        var forceOfGravity = G * _mass * otherBody._mass / (distance * distance);
        var unitVector = componentDistance / distance;
        var forceVector = unitVector * forceOfGravity;
        
        return forceVector;
    }

    /// <summary>
    /// A method used to update the positions and velocities of the bodies.
    /// </summary>
    /// <param name="bodies">A reference to the bodies data structure.</param>
    /// <param name="timeStep">The current simulation timestep.</param>
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

    /// <summary>
    /// Removes old trail positions.
    /// </summary>
    private void PruneTrails()
    {
        if (_orbitTrail.Count >= MaximumTrailLength)
        {
            _orbitTrail.RemoveAt(0);
        }
    }

    /// <summary>
    /// A method used to calculate the future positions of bodies. A list of virtual bodies is used to simplify the process.
    /// </summary>
    /// <param name="bodies">A reference to the bodies data structure.</param>
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

    /// <summary>
    /// Removes old orbit positions.
    /// </summary>
    private void PruneOrbits()
    {
        if (_futureOrbit.Count > OrbitCalculations)
        {
            _futureOrbit.RemoveRange(0, OrbitCalculations);
        }
    }
    
    /// <summary>
    /// The update method for the Body class, called in the SimulationScene.
    /// </summary>
    /// <param name="bodies">A reference to the bodies data structure.</param>
    /// <param name="userTimeStep">The current simulation timestep.</param>
    /// <param name="gameTime">A reference to the MonoGame GameTime class.</param>
    public void Update(List<Body> bodies, int userTimeStep, GameTime gameTime)
    {
        var timeStep = userTimeStep * (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        UpdateOrbits(bodies, timeStep);
        CalculateFutureOrbits(bodies);
        PruneTrails();
        PruneOrbits();
    }

    /// <summary>
    /// A method used to draw the trails of bodies.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="simulationMediator">A reference to a SimulationMediator class.</param>
    /// <param name="thickness">The desired thickness of the orbit trail.</param>
    private void DrawTrail(SpriteBatch spriteBatch, SimulationMediator simulationMediator, float thickness)
    {
        if (_orbitTrail.Count <= 1 || !simulationMediator.ToggleTrails) return;
        
        var trailLength = Math.Min(simulationMediator.TrailLength, _orbitTrail.Count);
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

    /// <summary>
    /// A method used to draw the future positions of bodies.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="simulationMediator">A reference to a SimulationMediator class.</param>
    /// <param name="thickness">The desired thickness of the orbit trail.</param>
    private void DrawOrbit(SpriteBatch spriteBatch, SimulationMediator simulationMediator, float thickness)
    {
        if (_futureOrbit.Count <= 1 || !simulationMediator.ToggleOrbits) return;
        
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

    /// <summary>
    /// A method used to draw a velocity vector arrow.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="color">The color of the arrow.</param>
    /// <param name="length">The length of the arrow stem.</param>
    /// <param name="width">The width of the arrow stem.</param>
    /// <param name="rotation">The rotation of the arrow.</param>
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

    /// <summary>
    /// A method used to draw the velocity vectors for the tangent component velocity, x component, and y component.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    private void DrawVelocityVectors(SpriteBatch spriteBatch, SimulationMediator simulationMediator)
    {
        if (!simulationMediator.ToggleVectors) return;
        
        var tangentVelocityAngle = MathF.Atan2(_velocity.Y, _velocity.X) - MathF.PI / 2;
        DrawArrow(spriteBatch, Color.White, 100, 2, tangentVelocityAngle);
        
        var xComponentVelocityAngle = MathF.PI * 1.5f;
        var xComponentArrowLength = _velocity.X * 15;
        DrawArrow(spriteBatch, Color.Red, xComponentArrowLength, 2, xComponentVelocityAngle);

        var yComponentVelocityAngle = 0f;
        var yComponentArrowLength = _velocity.Y * 15;
        DrawArrow(spriteBatch, Color.Green, yComponentArrowLength, 2, yComponentVelocityAngle);
    }

    /// <summary>
    /// A method used to draw a ring around a body to indicate to the user it is selected.
    /// </summary>
    /// <param name="shapeBatch">A reference to the Apos.Shapes ShapeBatch class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    private void DrawSelector(ShapeBatch shapeBatch, SimulationMediator simulationMediator)
    {
        if (!Selected || !simulationMediator.EditMode) return;
        
        var displayRadius = _displaySize * textureManager.BodyTexture.Width / 2;
        var selectorOffset = displayRadius / 5;
        const float miniMumOffset = 8.0f;
            
        if (selectorOffset < miniMumOffset) selectorOffset = miniMumOffset;
            
        var radius = displayRadius + selectorOffset;
            
        shapeBatch.Begin();
        shapeBatch.DrawCircle(_position, radius, Color.Transparent, Color.White, 2f);
        shapeBatch.End();
    }

    /// <summary>
    /// A method used to draw a glow visual effect around the body.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    private void DrawGlow(SpriteBatch spriteBatch, SimulationMediator simulationMediator)
    {
        if (!simulationMediator.ToggleGlow) return;
        
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

    /// <summary>
    /// A method used to draw the body proper.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <exception cref="InvalidOperationException">Return if a BodyTexture cannot be found.</exception>
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

    /// <summary>
    /// A method used to draw the names of bodies.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    private void DrawNames(SpriteBatch spriteBatch, SimulationMediator simulationMediator)
    {
        if (!simulationMediator.ToggleNames) return;

        var textSize = FontManager.MediumText(FontSize).MeasureString(_name);
        var padding = 10f;
        
        switch (simulationMediator.Position)
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

    /// <summary>
    /// The draw method for the Body class, called in the SimulationScene class.
    /// </summary>
    /// <param name="spriteBatch">A reference to the MonoGame SpriteBatch class.</param>
    /// <param name="shapeBatch">A reference to the Apos.Shapes ShapeBatch class.</param>
    /// <param name="simulationMediator">A reference to the SimulationMediator class.</param>
    public void Draw(SpriteBatch spriteBatch, ShapeBatch shapeBatch, SimulationMediator simulationMediator)
    {
        DrawTrail(spriteBatch, simulationMediator, TrailThickness);
        DrawOrbit(spriteBatch, simulationMediator, TrailThickness);
        DrawVelocityVectors(spriteBatch, simulationMediator);
        DrawBody(spriteBatch);
        DrawGlow(spriteBatch, simulationMediator);
        DrawSelector(shapeBatch, simulationMediator);
        DrawNames(spriteBatch, simulationMediator);
    }
}