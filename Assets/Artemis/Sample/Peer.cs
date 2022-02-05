using System;
using Artemis.Clients;
using Artemis.ValueObjects;
using EasyButtons;
using JetBrains.Annotations;
using UnityEngine;

namespace Artemis.Sample
{
    public class Peer : MonoBehaviour
    {
        private Address _recipient;
        private ArtemisClient _client;
        [SerializeField] private Peer _other;

        private void Awake()
        {
            _client = new ArtemisClient();
            _client.RegisterMessageHandler<Vehicle>(HandleVehicleMessage);
            _client.RegisterRequestHandler<DateTime>(HandleDateTimeRequest);
            _client.Start();
        }

        private void HandleVehicleMessage(Vehicle vehicle, Address sender)
        {
            Debug.Log($"Received a {vehicle.Brand}");
        }

        private void HandleDateTimeRequest(Request request, DateTime dateTime, Address sender)
        {
            _client.SendMessage(new Response(request, DateTime.UtcNow), sender, DeliveryMethod.Reliable);
        }

        private void Start()
        {
            _recipient = Address.FromHostname("localhost", _other._client.Port);
        }

        private void OnDestroy()
        {
            _client.Dispose();
        }

        [Button, UsedImplicitly]
        public async void Request()
        {
            var response = await _client.Request(new DateTime(), _recipient);
            Debug.Log(response);
        }

        [Button, UsedImplicitly]
        public void SendReliable()
        {
            var vehicle = new Vehicle {Brand = "Honda"};
            _client.SendMessage(vehicle, _recipient, DeliveryMethod.Reliable);
        }

        [Button, UsedImplicitly]
        public void SendUnreliable()
        {
            var vehicle = new Vehicle {Brand = "Honda"};
            _client.SendMessage(vehicle, _recipient, DeliveryMethod.Unreliable);
        }
    }
}