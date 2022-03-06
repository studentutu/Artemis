using Artemis.Clients;
using UnityEngine;

namespace Artemis.Samples.PingPong
{
    internal class PingServer : MonoBehaviour
    {
        private ArtemisClient _client;

        private void Start()
        {
            _client = new ArtemisClient(Configuration.ServerPort);
            _client.RegisterHandler(new PingRequestHandler());
            _client.Start();
        }

        private void OnDestroy()
        {
            _client.Dispose();
        }
    }
}