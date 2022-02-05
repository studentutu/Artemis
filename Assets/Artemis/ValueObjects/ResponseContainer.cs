using System;

namespace Artemis.ValueObjects
{
    [Serializable]
    public class ResponseContainer
    {
        public readonly string Id;
        public readonly object Payload;

        public ResponseContainer(string id, object payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}