﻿using Microsoft.Xna.Framework;

namespace _2dgs;

public class BodyData
{
    public string Name;
    public Vector2 Position { get; set; }
    public int Mass { get; set; }
    public float DisplayRadius { get; set; }
}