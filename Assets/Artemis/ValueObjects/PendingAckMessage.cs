namespace Artemis.ValueObjects
{
    public class PendingAckMessage
    {
        public readonly MessageContainer MessageContainer;
        public readonly Address Recepient;

        public PendingAckMessage(MessageContainer messageContainer, Address recepient)
        {
            MessageContainer = messageContainer;
            Recepient = recepient;
        }
    }
}