namespace Artemis.ValueObjects
{
    public class PendingAckPacket
    {
        public readonly Packet Packet;
        public readonly Address Recepient;

        public PendingAckPacket(Packet packet, Address recepient)
        {
            Packet = packet;
            Recepient = recepient;
        }
    }
}