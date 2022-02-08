using System;

namespace Artemis.Settings
{
    internal static class Configuration
    {
        internal static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(2);
        internal const int RetransmissionCapacity = 64;
        internal static readonly TimeSpan RetransmissionInterval = TimeSpan.FromMilliseconds(32);
    }
}