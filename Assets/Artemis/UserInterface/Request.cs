using System;
using System.Net;
using Artemis.Clients;
using Artemis.Packets;

namespace Artemis.UserInterface
{
    public readonly struct Request<TRequest>
    {
        public readonly TRequest Payload;
        public readonly IPEndPoint Sender;
        private readonly Guid _id;
        private readonly ArtemisClient _means;

        public Request(Guid id, TRequest request, IPEndPoint sender, ArtemisClient means)
        {
            _id = id;
            Payload = request;
            Sender = sender;
            _means = means;
        }

        public void Reply<TResponse>(TResponse response)
        {
            _means.SendReliableMessage(new Response(_id, response), Sender);
        }
    }
}