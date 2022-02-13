using System;

namespace Artemis.Sample.ValueObjects
{
    [Serializable]
    public struct Color
    {
        public float H;
        public float S;
        public float V;

        public static Color FromHSV(float h, float s, float v)
        {
            return new Color
            {
                H = h,
                S = s,
                V = v,
            };
        }

        public static implicit operator UnityEngine.Color(Color color)
        {
            return UnityEngine.Color.HSVToRGB(color.H, color.S, color.V);
        }
    }
}