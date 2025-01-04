using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace _2dgs;

public class GameStateManager
{
    private readonly Stack<GameState> _states = new();

    public void PushState(GameState state)
    {
        _states.Push(state);
    }

    public void PopState()
    {
        if (_states.Count > 0)
        {
            _states.Pop();
        }
    }

    public void ChangeState(GameState state)
    {
        if (_states.Count > 0)
        {
            _states.Pop();
        }
        _states.Push(state);
    }

    public void Update(GameTime gameTime)
    {
        if (_states.Count > 0)
        {
            _states.Peek().Update(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var state in _states.Reverse())
        {
            state.Draw(gameTime, spriteBatch);
        }
    }
}