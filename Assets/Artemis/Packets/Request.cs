using System;

namespace Artemis.Packets
{
    [Serializable]
    internal class Request
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