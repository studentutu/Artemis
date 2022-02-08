using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class ClientConnectedState : IClientState
    {
        void IClientState.OnStateEntered(Client client)
        {
            client._client.RegisterMessageHandler<ServerClosingMessage>(_ => HandleServerClosingMessage(client));
        }

        void IClientState.OnGUI(Client client)
        {
            GUILayoutUtilities.Button("Disconnect", () => Disconnect(client));
        }

        void IClientState.OnDestroy(Client client)
        {
            Disconnect(client);
        }

        private static void Disconnect(Client client)
        {
            client._client.SendMessage(
                new ClientDisconnectionMessage(),
                client.ServerAddress,
                DeliveryMethod.Unreliable);

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