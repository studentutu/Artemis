namespace rUDP
{
    public interface ISerializer
    {
        public byte[] Serialize<T>(T obj);
        public object Deserialize(byte[] bytes);
    }
}