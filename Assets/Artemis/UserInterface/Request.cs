using System;
using Artemis.Clients;
using Artemis.Packets;
using Artemis.ValueObjects;

namespace Artemis.UserInterface
{
    public readonly struct Request<TRequest>
    {
        public readonly TRequest Payload;
        public readonly Address Sender;
        private readonly Guid _id;
        private readonly ArtemisClient _means;

        public Request(Guid id, TRequest request, Address sender, ArtemisClient means)
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