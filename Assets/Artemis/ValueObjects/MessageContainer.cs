using System;

namespace Artemis.ValueObjects
{
    [Serializable]
    public class MessageContainer
    {
        public readonly int Sequence;
        public readonly object Payload;
        public readonly DeliveryMethod DeliveryMethod;

        public MessageContainer(int sequence, object payload, DeliveryMethod deliveryMethod)
        {
            Sequence = sequence;
            Payload = payload;
            DeliveryMethod = deliveryMethod;
        }
    }
}