using System;
using Artemis.ValueObjects;

namespace Artemis.Packets
{
    [Serializable]
    internal class Message
    {
        public readonly int Sequence;
        public readonly object Payload;
        public readonly DeliveryMethod DeliveryMethod;

        public Message(int sequence, object payload, DeliveryMethod deliveryMethod)
        {
            Sequence = sequence;
            Payload = payload;
            DeliveryMethod = deliveryMethod;
        }
    }
}