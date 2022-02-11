using System;
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
                client._client = new ArtemisClient(Array.Empty<Handler>());
                client._client.Start();
                client.ServerAddress = Address.FromHostname(host, Configuration.ServerPort);
                var ct = client.gameObject.GetOnDestroyCancellationToken();
                await client._client.RequestAsync(new ConnectionRequest(), client.ServerAddress, ct);
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
    }
}