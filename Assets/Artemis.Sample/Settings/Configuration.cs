using System;

namespace Artemis.Sample
{
    public static class Configuration
    {
        public const short ServerPort = 12345;
        public const int FixedUpdateRate = 8;
        public static readonly TimeSpan FixedDeltaTime = TimeSpan.FromSeconds(1f / FixedUpdateRate);
        public const double PlayerMovementSpeed = 2;
    }
}