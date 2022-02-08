using System;
using System.Threading.Tasks;
using Artemis.Clients;
using Artemis.Utilities;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class AClientDisconnectedState : AClientState
    {
        private string _host = "localhost";

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
                client.ServerAddress = Address.FromHostname(host, Constants.ServerPort);
                var ct = client.gameObject.GetOnDestroyCancellationToken();
                await client._client.RequestAsync(new ConnectionRequest(), client.ServerAddress, ct);
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