using System;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Clients;
using Artemis.Utilities;
using Artemis.ValueObjects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Artemis.Sample.Core
{
    public class ClientDisconnectedState : AClientState
    {
        private string _host = "localhost";

        public override void OnStateEntered(Client client)
        {
            Debug.Log($"[C] OnStateEntered {GetType().Name}");
        }

        public override void OnGUI(Client client)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayoutUtilities.Button("Connect", () => ConnectAsync(client, _host).Forget(), GUILayout.Width(64));
                _host = GUILayout.TextField(_host);
            }
        }

        private static async Task ConnectAsync(Client client, string host)
        {
            try
            {
                Debug.Log("Starting connection");
                client.Switch(client.Connecting);
                InitializeClient(client, host);
                var ct = client.gameObject.GetOnDestroyCancellationToken();
                var response = await RequestConnection(client, ct);
                client.PlayerId = response.PlayerId;
                Debug.LogError($"[C] Connected as {response.PlayerId}");
                await Object.FindObjectOfType<NetClock>().Synchronize(client, ct);
                Debug.Log("Starting completed");
                client.Switch(client.Connected);
            }
            catch (Exception e)
            {
                Debug.Log("Starting failed");
                if (e is not OperationCanceledException)
                {
                    Debug.LogError(e);
                }

                client.Switch(client.Disconnected);
            }
        }

        private static void InitializeClient(Client client, string serverHostname)
        {
            client._client = new ArtemisClient(Array.Empty<Handler>());
            client._client.Start();
            client.ServerAddress = Address.FromHostname(serverHostname, Configuration.ServerPort);
            client._client.RegisterHandler(new PlayerJoinedMessageHandler(client));
        }

        private static Task<ConnectionResponse> RequestConnection(Client client, CancellationToken ct)
        {
            var request = new ConnectionRequest(Environment.MachineName);
            return client._client
                .RequestAsync(request, client.ServerAddress, ct)
                .ContinueWith(t => (ConnectionResponse) t.Result, ct);
        }
    }
}