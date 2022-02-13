using System;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Clients;
using Artemis.Sample.Client.Handlers;
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

        public override void OnStateEntered(DapperClient dapperClient)
        {
            Debug.Log($"[C] OnStateEntered {GetType().Name}");
        }

        public override void OnGUI(DapperClient dapperClient)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayoutUtilities.Button("Connect", () => ConnectAsync(dapperClient, _host).Forget(), GUILayout.Width(64));
                _host = GUILayout.TextField(_host);
            }
        }

        private static async Task ConnectAsync(DapperClient dapperClient, string host)
        {
            try
            {
                dapperClient.Switch(dapperClient.Connecting);
                InitializeClient(dapperClient, host);
                var ct = dapperClient.gameObject.GetOnDestroyCancellationToken();
                var response = await RequestConnection(dapperClient, ct);
                dapperClient.PlayerId = response.PlayerId;
                await Object.FindObjectOfType<NetClock>().Synchronize(dapperClient, ct);
                dapperClient.Switch(dapperClient.Connected);
                
                UnityMainThreadDispatcher.Dispatch(() =>
                {
                    SpawnPlayer.Spawn(
                        response.PlayerId,
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

                dapperClient.Switch(dapperClient.Disconnected);
            }
        }

        private static void InitializeClient(DapperClient dapperClient, string serverHostname)
        {
            dapperClient._client = new ArtemisClient(Array.Empty<Handler>());
            dapperClient._client.Start();
            dapperClient.ServerAddress = Address.FromHostname(serverHostname, Configuration.ServerPort);
            dapperClient._client.RegisterHandler(new PlayerJoinedMessageHandler());
            dapperClient._client.RegisterHandler(new ClientDisconnectionMessageHandler());
        }

        private static Task<ConnectionResponse> RequestConnection(DapperClient dapperClient, CancellationToken ct)
        {
            var request = new ConnectionRequest(Environment.MachineName);
            return dapperClient._client
                .RequestAsync(request, dapperClient.ServerAddress, ct)
                .ContinueWith(t => (ConnectionResponse) t.Result, ct);
        }
    }
}