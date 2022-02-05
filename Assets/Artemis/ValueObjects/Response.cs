using System;

namespace Artemis.ValueObjects
{
    [Serializable]
    public class Response
    {
        public readonly string Id;
        public readonly object Payload;

        public Response(Request request, object payload)
        {
            Id = request.Id;
            Payload = payload;
        }
    }
}