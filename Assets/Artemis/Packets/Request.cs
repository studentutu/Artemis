using System;

namespace Artemis.Packets
{
    [Serializable]
    public class Request
    {
        public readonly Guid Id;
        public readonly object Payload;

        public Request(object payload)
        {
            Id = Guid.NewGuid();
            Payload = payload;
        }
    }
}