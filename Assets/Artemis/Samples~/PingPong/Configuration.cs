using System;

namespace Artemis.Samples.PingPong
{
    internal static class Configuration
    {
        internal const int ServerPort = 12345;
        private const int PingRate = 2;
        internal static readonly TimeSpan PingInterval = TimeSpan.FromSeconds(1f / PingRate);
    }
}