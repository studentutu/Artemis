using Artemis.ValueObjects;

namespace Artemis.UserInterface
{
    public readonly struct Message<T>
    {
        public readonly T Payload;
        public readonly Address Sender;

        public Message(T payload, Address sender)
        {
            Payload = payload;
            Sender = sender;
        }
    }
}