using rUDP.ValueObjects;

namespace rUDP.Sandbox
{
    public class PendingAckMessage
    {
        public readonly Message Message;
        public readonly Address Recepient;

        public PendingAckMessage(Message message, Address recepient)
        {
            Message = message;
            Recepient = recepient;
        }
    }
}