namespace rUDP.Sandbox
{
    public enum DeliveryMethod : byte
    {
        Reliable, // Ordered, duplicates discarded, guaranteed delivery
        Unreliable // Ordered, duplicates discarded
    }
}