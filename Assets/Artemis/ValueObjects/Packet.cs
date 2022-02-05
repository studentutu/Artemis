using System;

namespace Artemis.ValueObjects
{
    [Serializable]
    public class Packet
    {
        public readonly int Sequence;
        public readonly object Payload;
        public readonly DeliveryMethod DeliveryMethod;

        public Packet(int sequence, object payload, DeliveryMethod deliveryMethod)
        {
            Sequence = sequence;
            Payload = payload;
            DeliveryMethod = deliveryMethod;
        }
    }
}