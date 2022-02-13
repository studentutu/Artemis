using System;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Clients;
using Artemis.Sample.Player;
using Artemis.Threading;
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
                client.Switch(client.Connecting);
                InitializeClient(client, host);
                var ct = client.gameObject.GetOnDestroyCancellationToken();
                var response = await RequestConnection(client, ct);
                client.PlayerId = response.PlayerId;
                await Object.FindObjectOfType<NetClock>().Synchronize(client, ct);
                client.Switch(client.Connected);
                
                UnityMainThreadDispatcher.Dispatch(() =>
                {
                    InstantiatePlayer.Instantiate(
                        response.Nickname,
                        response.Color,
                        response.Position,
                        isLocalPlayer: true);
                });
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
            client._client.RegisterHandler(new PlayerJoinedMessageHandler());
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