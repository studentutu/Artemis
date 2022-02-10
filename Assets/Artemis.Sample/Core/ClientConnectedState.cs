using Artemis.ValueObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Artemis.Sample.Core
{
    public class ClientConnectedState : AClientState
    {
        public override void OnStateEntered(Client client)
        {
            Debug.Log($"[C] OnStateEntered {GetType().Name}");
            client._client.RegisterMessageHandler<ServerClosingMessage>(_ => HandleServerClosingMessage(client));
        }

        public override void OnGUI(Client client)
        {
            GUILayoutUtilities.Button("Disconnect", () => Disconnect(client));
        }

        public override void OnDestroy(Client client)
        {
            Debug.Log("OnDestroyClient");
            Disconnect(client);
        }

        private static void Disconnect(Client client)
        {
            Debug.Log($"Disco: {client.Current.GetType().Name}");
            Assert.IsNotNull(client);
            Assert.IsNotNull(client._client);
            client._client.SendUnreliableMessage(new ClientDisconnectionMessage(), client.ServerAddress);
            client._client.Dispose();
            client._client = null;
            client.Switch(client.Disconnected);
        }

        private void HandleServerClosingMessage(Client client)
        {
            Debug.Log("OnDestroyByServer");
            Disconnect(client);
        }
    }
}