using System;

namespace Artemis.Packets
{
    [Serializable]
    internal readonly struct Ack
    {
        internal readonly int Sequence;

        public Ack(int sequence)
        {
            Sequence = sequence;
        }
    }
}