namespace Artemis.ValueObjects
{
    public class Message<T>
    {
        public T Payload;
        public Address Sender;

        public Message(T payload, Address sender)
        {
            Payload = payload;
            Sender = sender;
        }
    }
}