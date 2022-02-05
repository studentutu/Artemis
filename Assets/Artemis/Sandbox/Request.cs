using System;

namespace rUDP.Sandbox
{
    [Serializable]
    public class Request
    {
        public readonly string Id;
        public readonly object Payload;

        public Request(object payload)
        {
            Id = Guid.NewGuid().ToString("N");
            Payload = payload;
        }
    }
}