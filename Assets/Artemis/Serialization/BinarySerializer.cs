using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace rUDP
{
    public class BinarySerializer : ISerializer
    {
        // private readonly MemoryStream _stream = new MemoryStream();
        // private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        //
        // public byte[] Serialize<T>(T obj)
        // {
        //     _stream.SetLength(0);
        //     _binaryFormatter.Serialize(_stream, obj);
        //     return _stream.ToArray();
        // }
        //
        // public object Deserialize(byte[] bytes)
        // {
        //     _stream.SetLength(0);
        //     _stream.Write(bytes, 0, bytes.Length);
        //     _stream.Position = 0;
        //     return _binaryFormatter.Deserialize(_stream);
        // }
        
        private static readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        
        public byte[] Serialize<T>(T obj)
        {
            using var stream = new MemoryStream();
            _binaryFormatter.Serialize(stream, obj);
            return stream.ToArray();
        }
        
        public object Deserialize(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return _binaryFormatter.Deserialize(stream);
        }
    }
}