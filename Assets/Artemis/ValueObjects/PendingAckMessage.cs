namespace Artemis.ValueObjects
{
    public class PendingAckMessage
    {
        public readonly Packet Packet;
        public readonly Address Recepient;

        public PendingAckMessage(Packet packet, Address recepient)
        {
            Packet = packet;
            Recepient = recepient;
        }
    }
}