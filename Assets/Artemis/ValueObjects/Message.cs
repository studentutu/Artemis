namespace Artemis.ValueObjects
{
    public class Message<T>
    {
        public T Payload;
        public Address Sender;
    }
}