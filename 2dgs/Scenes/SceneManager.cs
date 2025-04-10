using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

/// <summary>
/// A class that provides a means to manage multiple scenes.
/// </summary>
public class SceneManager
{
    /// <summary>
    /// A stack of the active scenes within the application.
    /// </summary>
    private readonly Stack<Scene> _scenes = new();

    /// <summary>
    /// Adds a new scene to the top of the _scenes stack.
    /// </summary>
    /// <param name="scene">The new scene to add.</param>
    public void PushScene(Scene scene)
    {
        _scenes.Push(scene);
    }

    /// <summary>
    /// Removes the top scene from the _scenes stack.
    /// </summary>
    public void PopScene()
    {
        if (_scenes.Count > 0)
        {
            _scenes.Pop();
        }
    }

    /// <summary>
    /// Removes all scenes and then pushes a new one.
    /// </summary>
    /// <param name="scene">The new scene to add.</param>
    public void ChangeScene(Scene scene)
    {
        if (_scenes.Count > 0)
        {
            _scenes.Pop();
        }
        _scenes.Push(scene);
    }

    /// <summary>
    /// Removes all scenes from the _scenes stack.
    /// </summary>
    public void ClearScenes()
    {
        _scenes.Clear();
    }

    /// <summary>
    /// Updates the scene at the top of the _scenes stack.
    /// </summary>
    /// <param name="gameTime">Access for MonoGame's GameTime class.</param>
    public void Update(GameTime gameTime)
    {
        if (_scenes.Count > 0)
        {
            _scenes.Peek().Update(gameTime);
        }
    }

    /// <summary>
    /// Draws the contents of the scene at the top of the _scenes stack.
    /// </summary>
    /// <param name="gameTime">A reference to MonoGame's GameTime class.</param>
    /// <param name="spriteBatch">A reference to MonoGame's SpriteBatch class.</param>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var scene in _scenes.Reverse())
        {
            scene.Draw(gameTime, spriteBatch);
        }
    }
}