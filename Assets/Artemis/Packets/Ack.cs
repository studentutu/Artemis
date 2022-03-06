using System;

namespace Artemis.Packets
{
    [Serializable]
    internal class Ack
    {
        public readonly int Sequence;

        public Ack(int sequence)
        {
            Sequence = sequence;
        }
    }
}