using System;

namespace Artemis.Packets
{
    [Serializable]
    public class Response
    {
        public readonly Guid Id;
        public readonly object Payload;

        public Response(Guid id, object payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}