namespace Artemis.Serialization
{
    internal interface ISerializer
    {
        byte[] Serialize<T>(T obj);
        object Deserialize(byte[] bytes);
    }
}