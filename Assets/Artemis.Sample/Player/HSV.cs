using System;
using UnityEngine;

namespace Artemis.Sample.Player
{
    [Serializable]
    public class HSV
    {
        public readonly float H;
        public readonly float S;
        public readonly float V;

        public HSV(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

        public Color ToUnityColor()
        {
            return Color.HSVToRGB(H, S, V);
        }
    }
}