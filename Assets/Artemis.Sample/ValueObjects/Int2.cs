using System;
using UnityEngine;

namespace Artemis.Sample.ValueObjects
{
    [Serializable]
    public struct Int2
    {
        public int X;
        public int Y;

        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static implicit operator Vector2(Int2 int2)
        {
            return new Vector2(int2.X, int2.Y);
        }

        public static implicit operator Vector2Int(Int2 int2)
        {
            return new Vector2Int(int2.X, int2.Y);
        }

        public static implicit operator Int2(Vector2Int vector2)
        {
            return new Int2(vector2.x, vector2.y);
        }
    }
}