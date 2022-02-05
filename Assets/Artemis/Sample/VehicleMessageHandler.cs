using Artemis.Clients;
using Artemis.UserInterface;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample
{
    public class VehicleMessageHandler : Handler
    {
        public override void Bind(ArtemisClient ac)
        {
            ac.RegisterMessageHandler<Vehicle>(Handle);
        }

        private static void Handle(Message<Vehicle> message)
        {
            Debug.Log($"Yayyy its a <b>{message.Payload.Brand}</b> from {message.Sender}");
        }
    }
}