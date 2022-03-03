using System.Net;

namespace Artemis.UserInterface
{
    public readonly struct Message<T>
    {
        public readonly T Payload;
        public readonly IPEndPoint Sender;

        public Message(T payload, IPEndPoint sender)
        {
            Payload = payload;
            Sender = sender;
        }
    }
}