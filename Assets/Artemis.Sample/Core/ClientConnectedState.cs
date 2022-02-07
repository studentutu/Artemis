using Artemis.UserInterface;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class ClientConnectedState : IClientState
    {
        void IClientState.OnStateEntered(Client client)
        {
            client._client.RegisterMessageHandler<ServerClosingMessage>(
                msg => HandleServerClosingMessage(client, msg));
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

        private void HandleServerClosingMessage(Client client, Message<ServerClosingMessage> message)
        {
            Debug.Log("<b>[C]</b> Server was closed");
            Disconnect(client);
        }
    }
}