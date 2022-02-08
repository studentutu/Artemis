using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class AClientConnectedState : AClientState
    {
        public override void OnStateEntered(Client client)
        {
            client._client.RegisterMessageHandler<ServerClosingMessage>(_ => HandleServerClosingMessage(client));
        }

        public override void OnGUI(Client client)
        {
            GUILayoutUtilities.Button("Disconnect", () => Disconnect(client));
        }

        public override void OnDestroy(Client client)
        {
            Disconnect(client);
        }

        private static void Disconnect(Client client)
        {
            client._client.SendUnreliableMessage(new ClientDisconnectionMessage(), client.ServerAddress);
            client._client.Dispose();
            client._client = null;
            client.Switch(client.Disconnected);
        }

        private void HandleServerClosingMessage(Client client)
        {
            Debug.Log("<b>[C]</b> Server was closed");
            Disconnect(client);
        }
    }
}