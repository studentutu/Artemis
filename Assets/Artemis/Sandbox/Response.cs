using System;

namespace rUDP.Sandbox
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