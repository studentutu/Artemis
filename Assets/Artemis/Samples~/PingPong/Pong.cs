using System;

namespace Artemis.Samples.PingPong
{
    [Serializable]
    internal struct Pong
    {
        internal static readonly Pong Instance = default(Pong);
    }
}