using System;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Clients;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Samples.PingPong
{
    internal class PingClient : MonoBehaviour
    {
        [Header("Readonly"), SerializeField] private string _roundTripTime = "Uninitialized";

        private ArtemisClient _client;
        private readonly Address _serverAddress = Address.FromHostname("localhost", Configuration.ServerPort);
        private readonly CancellationTokenSource _cts = new();

        private void Start()
        {
            _client = new ArtemisClient();
            _client.Start();
            Task.Run(() => PingServerLoop(_cts.Token), _cts.Token);
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
            _client.Dispose();
        }

        private async Task PingServerLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var rtt = await PingServer(ct);
                _roundTripTime = $"{rtt.TotalMilliseconds}ms";
                await Task.Delay(Configuration.PingInterval, ct);
            }
        }

        private async Task<TimeSpan> PingServer(CancellationToken ct)
        {
            var timeAtRequest = DateTime.Now;
            await _client.RequestAsync(Ping.Instance, _serverAddress, ct);
            var timeAtResponse = DateTime.Now;
            return timeAtResponse - timeAtRequest;
        }
    }
}