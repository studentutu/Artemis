using System;
using UnityEngine;
using Artemis.Clients;
using Artemis.ValueObjects;
using System.Collections.Generic;

namespace Artemis.Sample
{
    public class Peer : MonoBehaviour
    {
        private Address _recipient;
        private ArtemisClient _client;
        [SerializeField] private Peer _other;

        private readonly List<Handler> _handlers = new()
        {
            new DateRequestHandler(),
            new VehicleMessageHandler(),
        };

        private void Awake()
        {
            _client = new ArtemisClient(_handlers);
            _client.Start();
        }

        private void Start()
        {
            _recipient = Address.FromHostname("localhost", _other._client.Port);
        }

        private void OnDestroy()
        {
            _client.Dispose();
        }

        public void Present()
        {
            using (new GUILayout.VerticalScope("box", GUILayout.Width(160)))
            {
                GUILayout.Label(gameObject.name, new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter});
                if (GUILayout.Button("Request")) Request();
                if (GUILayout.Button("Send Reliable")) SendReliable();
                if (GUILayout.Button("Send Unreliable")) SendUnreliable();
                if (GUILayout.Button("Mass Send Reliable")) MassSendReliable();
            }
        }

        private async void Request()
        {
            Debug.Log(await _client.Request(new DateTime(), _recipient));
        }

        private void SendReliable()
        {
            _client.SendMessage(new Vehicle("Honda"), _recipient, DeliveryMethod.Reliable);
        }

        private void SendUnreliable()
        {
            _client.SendMessage(new Vehicle("Honda"), _recipient, DeliveryMethod.Unreliable);
        }

        private void MassSendReliable()
        {
            for (int i = 0; i < 1024; i++)
            {
                _client.SendMessage(new Vehicle("Honda"), _recipient, DeliveryMethod.Reliable);
            }
        }
    }
}