using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Artemis.Serialization
{
    internal class BinarySerializer : ISerializer
    {
        private static readonly BinaryFormatter _binaryFormatter = new();

        byte[] ISerializer.Serialize<T>(T obj)
        {
            using var stream = new MemoryStream();
            _binaryFormatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        object ISerializer.Deserialize(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return _binaryFormatter.Deserialize(stream);
        }
    }
}