using System;

namespace Artemis.Sample
{
    public static class Configuration
    {
        public const short ServerPort = 12345;
        public const int TicksPerSecond = 8;
        public static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(1f / TicksPerSecond);
        public const double PlayerMovementSpeed = 2;
    }
}