using System;

namespace Artemis.Samples.PingPong
{
    [Serializable]
    internal struct Ping
    {
        internal static readonly Ping Instance = default(Ping);
    }
}