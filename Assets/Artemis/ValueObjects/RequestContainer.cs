using System;

namespace Artemis.ValueObjects
{
    [Serializable]
    public class RequestContainer
    {
        public readonly string Id;
        public readonly object Payload;

        public RequestContainer(object payload)
        {
            Id = Guid.NewGuid().ToString("N");
            Payload = payload;
        }
    }
}