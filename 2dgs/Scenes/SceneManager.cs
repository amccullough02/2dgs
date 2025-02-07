using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class SceneManager
{
    private readonly Stack<Scene> _scenes = new();

    public void PushScene(Scene scene)
    {
        _scenes.Push(scene);
    }

    public void PopScene()
    {
        if (_scenes.Count > 0)
        {
            _scenes.Pop();
        }
    }

    public void ChangeScene(Scene scene)
    {
        if (_scenes.Count > 0)
        {
            _scenes.Pop();
        }
        _scenes.Push(scene);
    }

    public void Update(GameTime gameTime)
    {
        if (_scenes.Count > 0)
        {
            _scenes.Peek().Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var scene in _scenes.Reverse())
        {
            scene.Draw(gameTime, spriteBatch);
        }
    }
}