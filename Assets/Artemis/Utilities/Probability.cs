using System;

namespace Artemis.Utilities
{
    internal static class Probability
    {
        private static readonly Random _random = new Random(0);
        
        internal static bool Chance(double probability)
        {
            return _random.NextDouble() < probability;
        }
    }
}