using System;
using UnityEngine;

namespace Artemis.Sample.ValueObjects
{
    [Serializable]
    public struct Float2
    {
        public float X;
        public float Y;

        public Float2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(Float2 float2)
        {
            return new Vector2(float2.X, float2.Y);
        }

        public static implicit operator Float2(Vector2 vector2)
        {
            return new Float2(vector2.x, vector2.y);
        }
    }
}